using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Interfaces;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

// ── Queries ──

public class GetSchedulePeriodsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetSchedulePeriodsQuery, PagedResult<SchedulePeriodDto>>
{
    public async Task<PagedResult<SchedulePeriodDto>> Handle(GetSchedulePeriodsQuery r, CancellationToken ct)
    {
        var q = db.SchedulePeriods
            .Include(sp => sp.Location)
            .Include(sp => sp.ShiftAssignments)
            .Where(sp => sp.OrganizationId == r.OrgId);

        if (r.LocationId.HasValue) q = q.Where(sp => sp.LocationId == r.LocationId);

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(sp => sp.StartDate)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);

        return new PagedResult<SchedulePeriodDto>(
            mapper.Map<IReadOnlyList<SchedulePeriodDto>>(items), total, r.Page, r.PageSize,
            (int)Math.Ceiling(total / (double)r.PageSize));
    }
}

public class GetSchedulePeriodDetailHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetSchedulePeriodDetailQuery, SchedulePeriodDetailDto>
{
    public async Task<SchedulePeriodDetailDto> Handle(GetSchedulePeriodDetailQuery r, CancellationToken ct)
    {
        var sp = await db.SchedulePeriods
            .Include(s => s.Location)
            .Include(s => s.ShiftAssignments).ThenInclude(sa => sa.Employee)
            .Include(s => s.ShiftAssignments).ThenInclude(sa => sa.ShiftTemplate)
            .Include(s => s.ShiftAssignments).ThenInclude(sa => sa.Station)
            .Include(s => s.ShiftAssignments).ThenInclude(sa => sa.Role)
            .Include(s => s.StaffingRequirements).ThenInclude(sr => sr.Role)
            .Include(s => s.StaffingRequirements).ThenInclude(sr => sr.ShiftTemplate)
            .FirstOrDefaultAsync(s => s.Id == r.SchedulePeriodId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException($"Schedule period {r.SchedulePeriodId} not found");
        return mapper.Map<SchedulePeriodDetailDto>(sp);
    }
}

public class GetAssignmentsForDateRangeHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetAssignmentsForDateRangeQuery, IReadOnlyList<ShiftAssignmentDto>>
{
    public async Task<IReadOnlyList<ShiftAssignmentDto>> Handle(GetAssignmentsForDateRangeQuery r, CancellationToken ct)
    {
        var q = db.ShiftAssignments
            .Include(sa => sa.Employee).Include(sa => sa.ShiftTemplate)
            .Include(sa => sa.Station).Include(sa => sa.Role)
            .Where(sa => sa.OrganizationId == r.OrgId && sa.Date >= r.Start && sa.Date <= r.End);

        if (r.EmployeeId.HasValue) q = q.Where(sa => sa.EmployeeId == r.EmployeeId);

        var items = await q.OrderBy(sa => sa.Date).ThenBy(sa => sa.StartTime).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(items);
    }
}

public class CheckConflictsHandler(IConflictDetectionService conflictService)
    : IRequestHandler<CheckConflictsQuery, ConflictCheckResult>
{
    public async Task<ConflictCheckResult> Handle(CheckConflictsQuery r, CancellationToken ct)
    {
        return await conflictService.CheckAssignmentAsync(r.OrgId, r.EmployeeId, r.Date, r.StartTime, r.EndTime, r.ExcludeAssignmentId, ct);
    }
}

// ── Commands ──

public class CreateSchedulePeriodHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateSchedulePeriodCommand, SchedulePeriodDto>
{
    public async Task<SchedulePeriodDto> Handle(CreateSchedulePeriodCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var sp = new SchedulePeriod
        {
            OrganizationId = r.OrgId, LocationId = req.LocationId,
            StartDate = req.StartDate, EndDate = req.EndDate, Notes = req.Notes
        };
        db.SchedulePeriods.Add(sp);
        await db.SaveChangesAsync(ct);

        var loaded = await db.SchedulePeriods.Include(s => s.Location)
            .Include(s => s.ShiftAssignments)
            .FirstAsync(s => s.Id == sp.Id, ct);
        return mapper.Map<SchedulePeriodDto>(loaded);
    }
}

public class PublishScheduleHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<PublishScheduleCommand, SchedulePeriodDto>
{
    public async Task<SchedulePeriodDto> Handle(PublishScheduleCommand r, CancellationToken ct)
    {
        var sp = await db.SchedulePeriods.Include(s => s.Location).Include(s => s.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Id == r.SchedulePeriodId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Schedule period not found");

        sp.Status = SchedulePeriodStatus.Published;
        sp.PublishedAt = DateTime.UtcNow;
        sp.PublishedBy = r.PublishedBy;
        await db.SaveChangesAsync(ct);
        return mapper.Map<SchedulePeriodDto>(sp);
    }
}

public class LockScheduleHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<LockScheduleCommand, SchedulePeriodDto>
{
    public async Task<SchedulePeriodDto> Handle(LockScheduleCommand r, CancellationToken ct)
    {
        var sp = await db.SchedulePeriods.Include(s => s.Location).Include(s => s.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Id == r.SchedulePeriodId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Schedule period not found");

        sp.Status = SchedulePeriodStatus.Locked;
        sp.LockedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return mapper.Map<SchedulePeriodDto>(sp);
    }
}

public class CreateShiftAssignmentHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateShiftAssignmentCommand, ShiftAssignmentDto>
{
    public async Task<ShiftAssignmentDto> Handle(CreateShiftAssignmentCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var sa = new ShiftAssignment
        {
            OrganizationId = r.OrgId, SchedulePeriodId = req.SchedulePeriodId,
            EmployeeId = req.EmployeeId, ShiftTemplateId = req.ShiftTemplateId,
            StationId = req.StationId, RoleId = req.RoleId, Date = req.Date,
            StartTime = req.StartTime, EndTime = req.EndTime,
            BreakDurationMinutes = req.BreakDurationMinutes, BreakIsPaid = req.BreakIsPaid,
            Notes = req.Notes
        };
        db.ShiftAssignments.Add(sa);
        await db.SaveChangesAsync(ct);

        var loaded = await db.ShiftAssignments
            .Include(s => s.Employee).Include(s => s.ShiftTemplate)
            .Include(s => s.Station).Include(s => s.Role)
            .FirstAsync(s => s.Id == sa.Id, ct);
        return mapper.Map<ShiftAssignmentDto>(loaded);
    }
}

public class UpdateShiftAssignmentHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateShiftAssignmentCommand, ShiftAssignmentDto>
{
    public async Task<ShiftAssignmentDto> Handle(UpdateShiftAssignmentCommand r, CancellationToken ct)
    {
        var sa = await db.ShiftAssignments.FirstOrDefaultAsync(s => s.Id == r.AssignmentId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Shift assignment not found");

        var req = r.Dto;
        sa.ShiftTemplateId = req.ShiftTemplateId; sa.StationId = req.StationId;
        sa.RoleId = req.RoleId; sa.StartTime = req.StartTime; sa.EndTime = req.EndTime;
        sa.BreakDurationMinutes = req.BreakDurationMinutes; sa.Notes = req.Notes;
        await db.SaveChangesAsync(ct);

        var loaded = await db.ShiftAssignments
            .Include(s => s.Employee).Include(s => s.ShiftTemplate)
            .Include(s => s.Station).Include(s => s.Role)
            .FirstAsync(s => s.Id == sa.Id, ct);
        return mapper.Map<ShiftAssignmentDto>(loaded);
    }
}

public class DeleteShiftAssignmentHandler(StaffProDbContext db)
    : IRequestHandler<DeleteShiftAssignmentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteShiftAssignmentCommand r, CancellationToken ct)
    {
        var sa = await db.ShiftAssignments.FirstOrDefaultAsync(s => s.Id == r.AssignmentId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Shift assignment not found");
        db.ShiftAssignments.Remove(sa);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CopyScheduleHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CopyScheduleCommand, SchedulePeriodDto>
{
    public async Task<SchedulePeriodDto> Handle(CopyScheduleCommand r, CancellationToken ct)
    {
        var source = await db.SchedulePeriods
            .Include(s => s.ShiftAssignments).Include(s => s.Location)
            .FirstOrDefaultAsync(s => s.Id == r.Dto.SourceSchedulePeriodId && s.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Source schedule not found");

        var dayOffset = r.Dto.NewStartDate.DayNumber - source.StartDate.DayNumber;
        var newEnd = source.EndDate.AddDays(dayOffset);

        var newSp = new SchedulePeriod
        {
            OrganizationId = r.OrgId, LocationId = source.LocationId,
            StartDate = r.Dto.NewStartDate, EndDate = newEnd
        };
        db.SchedulePeriods.Add(newSp);

        foreach (var sa in source.ShiftAssignments)
        {
            db.ShiftAssignments.Add(new ShiftAssignment
            {
                OrganizationId = r.OrgId, SchedulePeriodId = newSp.Id,
                EmployeeId = sa.EmployeeId, ShiftTemplateId = sa.ShiftTemplateId,
                StationId = sa.StationId, RoleId = sa.RoleId,
                Date = sa.Date.AddDays(dayOffset), StartTime = sa.StartTime, EndTime = sa.EndTime,
                BreakDurationMinutes = sa.BreakDurationMinutes, BreakIsPaid = sa.BreakIsPaid
            });
        }

        await db.SaveChangesAsync(ct);
        var loaded = await db.SchedulePeriods.Include(s => s.Location).Include(s => s.ShiftAssignments)
            .FirstAsync(s => s.Id == newSp.Id, ct);
        return mapper.Map<SchedulePeriodDto>(loaded);
    }
}

public class CreateStaffingRequirementHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateStaffingRequirementCommand, StaffingRequirementDto>
{
    public async Task<StaffingRequirementDto> Handle(CreateStaffingRequirementCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var sr = new StaffingRequirement
        {
            OrganizationId = r.OrgId, SchedulePeriodId = req.SchedulePeriodId,
            RoleId = req.RoleId, DayOfWeek = req.DayOfWeek, ShiftTemplateId = req.ShiftTemplateId,
            MinStaff = req.MinStaff, MaxStaff = req.MaxStaff
        };
        db.StaffingRequirements.Add(sr);
        await db.SaveChangesAsync(ct);

        var loaded = await db.StaffingRequirements
            .Include(s => s.Role).Include(s => s.ShiftTemplate)
            .FirstAsync(s => s.Id == sr.Id, ct);
        return mapper.Map<StaffingRequirementDto>(loaded);
    }
}
