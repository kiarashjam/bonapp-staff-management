using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

public class GetManagerDashboardHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetManagerDashboardQuery, ManagerDashboardDto>
{
    public async Task<ManagerDashboardDto> Handle(GetManagerDashboardQuery r, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        var weekEnd = weekStart.AddDays(6);

        var totalEmployees = await db.Employees.CountAsync(e => e.OrganizationId == r.OrgId && e.Status == EmployeeStatus.Active, ct);

        var todayShifts = await db.ShiftAssignments
            .Include(sa => sa.Employee).Include(sa => sa.ShiftTemplate)
            .Include(sa => sa.Station).Include(sa => sa.Role)
            .Where(sa => sa.OrganizationId == r.OrgId && sa.Date == today)
            .ToListAsync(ct);

        var onLeaveToday = await db.TimeOffRequests.CountAsync(
            t => t.OrganizationId == r.OrgId && t.Status == TimeOffRequestStatus.Approved
                 && t.StartDate <= today && t.EndDate >= today, ct);

        var pendingTimeOff = await db.TimeOffRequests.CountAsync(
            t => t.OrganizationId == r.OrgId && t.Status == TimeOffRequestStatus.Pending, ct);

        var pendingTimesheets = await db.Timesheets.CountAsync(
            t => t.OrganizationId == r.OrgId && t.Status == TimesheetStatus.EmployeeSubmitted, ct);

        var weekShifts = await db.ShiftAssignments
            .Where(sa => sa.OrganizationId == r.OrgId && sa.Date >= weekStart && sa.Date <= weekEnd)
            .ToListAsync(ct);
        var weeklyHours = (decimal)weekShifts.Sum(s => s.NetHours);

        return new ManagerDashboardDto(
            totalEmployees, todayShifts.Select(s => s.EmployeeId).Distinct().Count(),
            onLeaveToday, pendingTimeOff, pendingTimesheets, 0, weeklyHours,
            mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(todayShifts));
    }
}

public class GetEmployeeDashboardHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetEmployeeDashboardQuery, EmployeeDashboardDto>
{
    public async Task<EmployeeDashboardDto> Handle(GetEmployeeDashboardQuery r, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        var weekEnd = weekStart.AddDays(6);

        var clockStatus = await new GetClockStatusHandler(db)
            .Handle(new GetClockStatusQuery(r.OrgId, r.EmployeeId), ct);

        var upcomingShifts = await db.ShiftAssignments
            .Include(sa => sa.Employee).Include(sa => sa.ShiftTemplate)
            .Include(sa => sa.Station).Include(sa => sa.Role)
            .Where(sa => sa.EmployeeId == r.EmployeeId && sa.Date >= today)
            .OrderBy(sa => sa.Date).ThenBy(sa => sa.StartTime)
            .Take(10).ToListAsync(ct);

        var leaveBalances = await db.Set<Domain.Entities.LeaveBalance>()
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.EmployeeId == r.EmployeeId && lb.Year == DateTime.UtcNow.Year)
            .ToListAsync(ct);

        var weekShifts = await db.ShiftAssignments
            .Where(sa => sa.EmployeeId == r.EmployeeId && sa.Date >= weekStart && sa.Date <= weekEnd)
            .ToListAsync(ct);
        var hoursThisWeek = (decimal)weekShifts.Sum(s => s.NetHours);

        var periodStart = today.AddDays(-today.Day + 1);
        var periodEnd = periodStart.AddMonths(1).AddDays(-1);
        var periodShifts = await db.ShiftAssignments
            .Where(sa => sa.EmployeeId == r.EmployeeId && sa.Date >= periodStart && sa.Date <= periodEnd)
            .ToListAsync(ct);
        var hoursThisPeriod = (decimal)periodShifts.Sum(s => s.NetHours);

        var pendingRequests = await db.TimeOffRequests.CountAsync(
            t => t.EmployeeId == r.EmployeeId && t.Status == TimeOffRequestStatus.Pending, ct);

        return new EmployeeDashboardDto(
            clockStatus,
            mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(upcomingShifts),
            mapper.Map<IReadOnlyList<LeaveBalanceDto>>(leaveBalances),
            hoursThisWeek, hoursThisPeriod, pendingRequests);
    }
}
