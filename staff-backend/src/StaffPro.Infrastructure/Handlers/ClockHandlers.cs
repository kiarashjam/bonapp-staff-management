using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

// ── Clock Queries ──

public class GetClockStatusHandler(StaffProDbContext db)
    : IRequestHandler<GetClockStatusQuery, ClockStatusDto>
{
    public async Task<ClockStatusDto> Handle(GetClockStatusQuery r, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var entries = await db.ClockEntries
            .Where(c => c.EmployeeId == r.EmployeeId && DateOnly.FromDateTime(c.Timestamp) == today)
            .OrderBy(c => c.Timestamp).ToListAsync(ct);

        var lastEntry = entries.LastOrDefault();
        var isClockedIn = lastEntry?.EntryType == ClockEntryType.ClockIn || lastEntry?.EntryType == ClockEntryType.BreakEnd;
        var isOnBreak = lastEntry?.EntryType == ClockEntryType.BreakStart;
        var lastClockIn = entries.LastOrDefault(e => e.EntryType == ClockEntryType.ClockIn)?.Timestamp;
        var lastBreakStart = entries.LastOrDefault(e => e.EntryType == ClockEntryType.BreakStart)?.Timestamp;

        TimeSpan? worked = null;
        if (lastClockIn.HasValue)
        {
            var clockOut = entries.LastOrDefault(e => e.EntryType == ClockEntryType.ClockOut)?.Timestamp;
            worked = (clockOut ?? DateTime.UtcNow) - lastClockIn.Value;
        }

        return new ClockStatusDto(isClockedIn, isOnBreak, lastClockIn, lastBreakStart, null, worked);
    }
}

public class GetClockEntriesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetClockEntriesQuery, IReadOnlyList<ClockEntryDto>>
{
    public async Task<IReadOnlyList<ClockEntryDto>> Handle(GetClockEntriesQuery r, CancellationToken ct)
    {
        var items = await db.ClockEntries
            .Where(c => c.EmployeeId == r.EmployeeId && DateOnly.FromDateTime(c.Timestamp) == r.Date)
            .OrderByDescending(c => c.Timestamp).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<ClockEntryDto>>(items);
    }
}

// ── Clock Commands ──

public class ClockActionHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<ClockActionCommand, ClockEntryDto>
{
    public async Task<ClockEntryDto> Handle(ClockActionCommand r, CancellationToken ct)
    {
        var entry = new ClockEntry
        {
            OrganizationId = r.OrgId, EmployeeId = r.EmployeeId,
            EntryType = r.Dto.EntryType, Timestamp = DateTime.UtcNow,
            Source = ClockSource.WebApp, Latitude = r.Dto.Latitude,
            Longitude = r.Dto.Longitude, GpsAccuracyMeters = r.Dto.GpsAccuracyMeters,
            IpAddress = r.Dto.IpAddress
        };
        db.ClockEntries.Add(entry);
        await db.SaveChangesAsync(ct);
        return mapper.Map<ClockEntryDto>(entry);
    }
}

public class ManualClockOverrideHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<ManualClockOverrideCommand, ClockEntryDto>
{
    public async Task<ClockEntryDto> Handle(ManualClockOverrideCommand r, CancellationToken ct)
    {
        var entry = new ClockEntry
        {
            OrganizationId = r.OrgId, EmployeeId = r.Dto.EmployeeId,
            EntryType = r.Dto.EntryType, Timestamp = r.Dto.Timestamp,
            Source = ClockSource.ManagerOverride, IsManualOverride = true,
            OverrideReason = r.Dto.Reason, OverrideBy = r.OverrideBy
        };
        db.ClockEntries.Add(entry);
        await db.SaveChangesAsync(ct);
        return mapper.Map<ClockEntryDto>(entry);
    }
}

// ── Timesheet Queries ──

public class GetTimesheetsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetTimesheetsQuery, PagedResult<TimesheetDto>>
{
    public async Task<PagedResult<TimesheetDto>> Handle(GetTimesheetsQuery r, CancellationToken ct)
    {
        var q = db.Timesheets.Include(t => t.Employee)
            .Where(t => t.OrganizationId == r.OrgId);

        if (r.EmployeeId.HasValue) q = q.Where(t => t.EmployeeId == r.EmployeeId);
        if (!string.IsNullOrWhiteSpace(r.Status) && Enum.TryParse<TimesheetStatus>(r.Status, out var status))
            q = q.Where(t => t.Status == status);

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(t => t.PeriodStart)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);

        return new PagedResult<TimesheetDto>(
            mapper.Map<IReadOnlyList<TimesheetDto>>(items), total, r.Page, r.PageSize,
            (int)Math.Ceiling(total / (double)r.PageSize));
    }
}

public class GetTimesheetDetailHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetTimesheetDetailQuery, TimesheetDetailDto>
{
    public async Task<TimesheetDetailDto> Handle(GetTimesheetDetailQuery r, CancellationToken ct)
    {
        var ts = await db.Timesheets
            .Include(t => t.Employee).Include(t => t.Entries)
            .FirstOrDefaultAsync(t => t.Id == r.TimesheetId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Timesheet not found");
        return mapper.Map<TimesheetDetailDto>(ts);
    }
}

public class GetPayrollSummaryHandler(StaffProDbContext db)
    : IRequestHandler<GetPayrollSummaryQuery, IReadOnlyList<PayrollExportDto>>
{
    public async Task<IReadOnlyList<PayrollExportDto>> Handle(GetPayrollSummaryQuery r, CancellationToken ct)
    {
        var timesheets = await db.Timesheets
            .Include(t => t.Employee).ThenInclude(e => e.Contracts)
            .Where(t => t.OrganizationId == r.OrgId && t.PeriodStart >= r.PeriodStart && t.PeriodEnd <= r.PeriodEnd)
            .ToListAsync(ct);

        return timesheets.Select(t =>
        {
            var contract = t.Employee.Contracts.FirstOrDefault(c => c.IsActive);
            return new PayrollExportDto(
                t.EmployeeId, t.Employee.FullName, t.Employee.Email,
                contract?.ContractType.ToString(), contract?.HourlyRate ?? 0,
                t.RegularHours, t.OvertimeHours, t.RegularPayCents / 100m,
                t.OvertimePayCents / 100m, t.TotalGrossPayCents / 100m,
                t.PeriodStart, t.PeriodEnd);
        }).ToList();
    }
}

// ── Timesheet Commands ──

public class SubmitTimesheetHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<SubmitTimesheetCommand, TimesheetDto>
{
    public async Task<TimesheetDto> Handle(SubmitTimesheetCommand r, CancellationToken ct)
    {
        var ts = await db.Timesheets.Include(t => t.Employee)
            .FirstOrDefaultAsync(t => t.Id == r.TimesheetId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Timesheet not found");
        ts.Status = TimesheetStatus.EmployeeSubmitted;
        ts.SubmittedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return mapper.Map<TimesheetDto>(ts);
    }
}

public class ApproveTimesheetHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<ApproveTimesheetCommand, TimesheetDto>
{
    public async Task<TimesheetDto> Handle(ApproveTimesheetCommand r, CancellationToken ct)
    {
        var ts = await db.Timesheets.Include(t => t.Employee)
            .FirstOrDefaultAsync(t => t.Id == r.TimesheetId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Timesheet not found");
        ts.Status = TimesheetStatus.ManagerApproved;
        ts.ApprovedAt = DateTime.UtcNow;
        ts.ApprovedBy = r.ApprovedBy;
        ts.Notes = r.Notes;
        await db.SaveChangesAsync(ct);
        return mapper.Map<TimesheetDto>(ts);
    }
}
