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

public class GetTimeOffRequestsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetTimeOffRequestsQuery, PagedResult<TimeOffRequestDto>>
{
    public async Task<PagedResult<TimeOffRequestDto>> Handle(GetTimeOffRequestsQuery r, CancellationToken ct)
    {
        var q = db.TimeOffRequests
            .Include(t => t.Employee).Include(t => t.LeaveType)
            .Where(t => t.OrganizationId == r.OrgId);

        if (r.EmployeeId.HasValue) q = q.Where(t => t.EmployeeId == r.EmployeeId);
        if (!string.IsNullOrWhiteSpace(r.Status) && Enum.TryParse<TimeOffRequestStatus>(r.Status, out var status))
            q = q.Where(t => t.Status == status);

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(t => t.CreatedAt)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);

        return new PagedResult<TimeOffRequestDto>(
            mapper.Map<IReadOnlyList<TimeOffRequestDto>>(items), total, r.Page, r.PageSize,
            (int)Math.Ceiling(total / (double)r.PageSize));
    }
}

public class GetTimeOffRequestByIdHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetTimeOffRequestByIdQuery, TimeOffRequestDto>
{
    public async Task<TimeOffRequestDto> Handle(GetTimeOffRequestByIdQuery r, CancellationToken ct)
    {
        var req = await db.TimeOffRequests
            .Include(t => t.Employee).Include(t => t.LeaveType)
            .FirstOrDefaultAsync(t => t.Id == r.RequestId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Time off request not found");
        return mapper.Map<TimeOffRequestDto>(req);
    }
}

public class GetStaffingImpactHandler(StaffProDbContext db)
    : IRequestHandler<GetStaffingImpactQuery, IReadOnlyList<StaffingImpactDto>>
{
    public async Task<IReadOnlyList<StaffingImpactDto>> Handle(GetStaffingImpactQuery r, CancellationToken ct)
    {
        var results = new List<StaffingImpactDto>();
        for (var d = r.StartDate; d <= r.EndDate; d = d.AddDays(1))
        {
            var scheduled = await db.ShiftAssignments.CountAsync(
                sa => sa.OrganizationId == r.OrgId && sa.Date == d, ct);
            var onLeave = await db.TimeOffRequests.CountAsync(
                t => t.OrganizationId == r.OrgId && t.Status == TimeOffRequestStatus.Approved
                     && t.StartDate <= d && t.EndDate >= d, ct);
            results.Add(new StaffingImpactDto(d, scheduled, onLeave, 1, scheduled - onLeave >= 1));
        }
        return results;
    }
}

public class GetLeaveBalancesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetLeaveBalancesQuery, IReadOnlyList<LeaveBalanceDto>>
{
    public async Task<IReadOnlyList<LeaveBalanceDto>> Handle(GetLeaveBalancesQuery r, CancellationToken ct)
    {
        var items = await db.Set<LeaveBalance>()
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.EmployeeId == r.EmployeeId && lb.Year == (r.Year ?? DateTime.UtcNow.Year))
            .ToListAsync(ct);
        return mapper.Map<IReadOnlyList<LeaveBalanceDto>>(items);
    }
}

// ── Commands ──

public class CreateTimeOffRequestHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateTimeOffRequestCommand, TimeOffRequestDto>
{
    public async Task<TimeOffRequestDto> Handle(CreateTimeOffRequestCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var tor = new TimeOffRequest
        {
            OrganizationId = r.OrgId, EmployeeId = req.EmployeeId, LeaveTypeId = req.LeaveTypeId,
            StartDate = req.StartDate, EndDate = req.EndDate,
            StartTime = req.StartTime, EndTime = req.EndTime, Reason = req.Reason
        };
        db.TimeOffRequests.Add(tor);
        await db.SaveChangesAsync(ct);

        var loaded = await db.TimeOffRequests
            .Include(t => t.Employee).Include(t => t.LeaveType)
            .FirstAsync(t => t.Id == tor.Id, ct);
        return mapper.Map<TimeOffRequestDto>(loaded);
    }
}

public class ReviewTimeOffRequestHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<ReviewTimeOffRequestCommand, TimeOffRequestDto>
{
    public async Task<TimeOffRequestDto> Handle(ReviewTimeOffRequestCommand r, CancellationToken ct)
    {
        var tor = await db.TimeOffRequests
            .Include(t => t.Employee).Include(t => t.LeaveType)
            .FirstOrDefaultAsync(t => t.Id == r.RequestId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Time off request not found");

        tor.Status = r.Dto.Approved ? TimeOffRequestStatus.Approved : TimeOffRequestStatus.Denied;
        tor.ReviewedAt = DateTime.UtcNow;
        tor.ReviewedBy = r.ReviewedBy;
        tor.DenialReason = r.Dto.DenialReason;

        if (r.Dto.Approved)
        {
            var balance = await db.Set<LeaveBalance>()
                .FirstOrDefaultAsync(lb => lb.EmployeeId == tor.EmployeeId
                    && lb.LeaveTypeId == tor.LeaveTypeId && lb.Year == tor.StartDate.Year, ct);
            if (balance != null) balance.Used += tor.TotalDays;
        }

        await db.SaveChangesAsync(ct);
        return mapper.Map<TimeOffRequestDto>(tor);
    }
}

public class CancelTimeOffRequestHandler(StaffProDbContext db)
    : IRequestHandler<CancelTimeOffRequestCommand, Unit>
{
    public async Task<Unit> Handle(CancelTimeOffRequestCommand r, CancellationToken ct)
    {
        var tor = await db.TimeOffRequests.FirstOrDefaultAsync(t => t.Id == r.RequestId && t.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Time off request not found");
        tor.Status = TimeOffRequestStatus.Cancelled;
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
