using Microsoft.EntityFrameworkCore;
using StaffPro.Application.DTOs;
using StaffPro.Application.Interfaces;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Services;

public class ConflictDetectionService : IConflictDetectionService
{
    private readonly StaffProDbContext _db;

    public ConflictDetectionService(StaffProDbContext db) => _db = db;

    public async Task<ConflictCheckResult> CheckAssignmentAsync(
        Guid orgId, Guid employeeId, DateOnly date, TimeOnly startTime, TimeOnly endTime,
        Guid? excludeAssignmentId = null, CancellationToken ct = default)
    {
        var conflicts = new List<ConflictDto>();
        var org = await _db.Organizations.FindAsync([orgId], ct);
        if (org == null) return new ConflictCheckResult(true, [new ConflictDto("ORG_NOT_FOUND", "Organization not found", ConflictSeverity.Error, employeeId)]);

        var employee = await _db.Employees
            .Include(e => e.Contracts)
            .Include(e => e.EmployeeRoles)
            .FirstOrDefaultAsync(e => e.Id == employeeId && e.OrganizationId == orgId, ct);

        if (employee == null)
        {
            conflicts.Add(new ConflictDto("EMPLOYEE_NOT_FOUND", "Employee not found", ConflictSeverity.Error, employeeId));
            return new ConflictCheckResult(true, conflicts);
        }

        // 1. Check employee is active
        if (employee.Status != EmployeeStatus.Active)
            conflicts.Add(new ConflictDto("EMPLOYEE_INACTIVE", $"Employee {employee.FullName} is {employee.Status}", ConflictSeverity.Error, employeeId));

        // 2. Check availability
        var availability = await _db.RecurringAvailabilities
            .Where(a => a.EmployeeId == employeeId && a.DayOfWeek == date.DayOfWeek)
            .ToListAsync(ct);

        var overrideAvail = await _db.AvailabilityOverrides
            .Where(a => a.EmployeeId == employeeId && a.Date == date)
            .FirstOrDefaultAsync(ct);

        if (overrideAvail != null && overrideAvail.Type == AvailabilityType.Unavailable)
        {
            conflicts.Add(new ConflictDto("UNAVAILABLE_OVERRIDE", $"Employee has an unavailability override on {date}", ConflictSeverity.Error, employeeId));
        }
        else if (overrideAvail == null && availability.Any(a => a.Type == AvailabilityType.Unavailable))
        {
            conflicts.Add(new ConflictDto("UNAVAILABLE_RECURRING", $"Employee is unavailable on {date.DayOfWeek}s", ConflictSeverity.Error, employeeId));
        }

        // 3. Check overlapping shifts
        var existingShifts = await _db.ShiftAssignments
            .Where(sa => sa.EmployeeId == employeeId && sa.Date == date &&
                         sa.Status != ShiftAssignmentStatus.Cancelled &&
                         (excludeAssignmentId == null || sa.Id != excludeAssignmentId))
            .ToListAsync(ct);

        foreach (var existing in existingShifts)
        {
            if (TimesOverlap(startTime, endTime, existing.StartTime, existing.EndTime))
            {
                conflicts.Add(new ConflictDto("OVERLAP", $"Overlaps with existing shift {existing.StartTime}-{existing.EndTime}", ConflictSeverity.Error, employeeId));
            }
        }

        // 4. Check minimum rest between shifts
        var previousDay = date.AddDays(-1);
        var nextDay = date.AddDays(1);

        var adjacentShifts = await _db.ShiftAssignments
            .Where(sa => sa.EmployeeId == employeeId &&
                         (sa.Date == previousDay || sa.Date == nextDay) &&
                         sa.Status != ShiftAssignmentStatus.Cancelled &&
                         (excludeAssignmentId == null || sa.Id != excludeAssignmentId))
            .ToListAsync(ct);

        foreach (var adj in adjacentShifts)
        {
            double restHours;
            if (adj.Date == previousDay)
            {
                restHours = (date.ToDateTime(startTime) - previousDay.ToDateTime(TimeOnly.FromTimeSpan(adj.EndTime.ToTimeSpan()))).TotalHours;
            }
            else
            {
                restHours = (nextDay.ToDateTime(TimeOnly.FromTimeSpan(adj.StartTime.ToTimeSpan())) - date.ToDateTime(endTime)).TotalHours;
            }

            if (restHours < org.MinRestHoursBetweenShifts)
                conflicts.Add(new ConflictDto("INSUFFICIENT_REST", $"Only {restHours:F1}h rest (minimum {org.MinRestHoursBetweenShifts}h required)", ConflictSeverity.Error, employeeId));
        }

        // 5. Check approved time off
        var timeOff = await _db.TimeOffRequests
            .Where(tr => tr.EmployeeId == employeeId && tr.Status == TimeOffRequestStatus.Approved &&
                         tr.StartDate <= date && tr.EndDate >= date)
            .AnyAsync(ct);

        if (timeOff)
            conflicts.Add(new ConflictDto("ON_LEAVE", "Employee has approved time off on this date", ConflictSeverity.Error, employeeId));

        // 6. Check weekly hours limit
        var weekStart = date.AddDays(-(int)date.DayOfWeek + 1); // Monday
        var weekEnd = weekStart.AddDays(6);
        var weeklyShifts = await _db.ShiftAssignments
            .Where(sa => sa.EmployeeId == employeeId && sa.Date >= weekStart && sa.Date <= weekEnd &&
                         sa.Status != ShiftAssignmentStatus.Cancelled &&
                         (excludeAssignmentId == null || sa.Id != excludeAssignmentId))
            .ToListAsync(ct);

        var shiftDuration = (endTime.ToTimeSpan() - startTime.ToTimeSpan()).TotalHours;
        if (shiftDuration < 0) shiftDuration += 24;
        var weeklyTotal = weeklyShifts.Sum(s => s.NetHours) + shiftDuration;

        if (weeklyTotal > org.MaxHoursPerWeek)
            conflicts.Add(new ConflictDto("WEEKLY_HOURS_EXCEEDED", $"Weekly hours would be {weeklyTotal:F1}h (max {org.MaxHoursPerWeek}h)", ConflictSeverity.Warning, employeeId));

        // 7. Check daily hours limit
        var dailyTotal = existingShifts.Sum(s => s.TotalHours) + shiftDuration;
        if (dailyTotal > org.MaxHoursPerDay)
            conflicts.Add(new ConflictDto("DAILY_HOURS_EXCEEDED", $"Daily hours would be {dailyTotal:F1}h (max {org.MaxHoursPerDay}h)", ConflictSeverity.Warning, employeeId));

        // 8. Check consecutive working days
        var consecutiveDays = 1;
        for (var d = date.AddDays(-1); d >= date.AddDays(-org.MaxConsecutiveWorkDays); d = d.AddDays(-1))
        {
            var hasShift = await _db.ShiftAssignments
                .AnyAsync(sa => sa.EmployeeId == employeeId && sa.Date == d &&
                               sa.Status != ShiftAssignmentStatus.Cancelled, ct);
            if (hasShift) consecutiveDays++;
            else break;
        }

        if (consecutiveDays > org.MaxConsecutiveWorkDays)
            conflicts.Add(new ConflictDto("CONSECUTIVE_DAYS_EXCEEDED", $"{consecutiveDays} consecutive days (max {org.MaxConsecutiveWorkDays})", ConflictSeverity.Warning, employeeId));

        return new ConflictCheckResult(conflicts.Any(c => c.Severity == ConflictSeverity.Error), conflicts);
    }

    private static bool TimesOverlap(TimeOnly s1, TimeOnly e1, TimeOnly s2, TimeOnly e2)
    {
        return s1 < e2 && s2 < e1;
    }
}
