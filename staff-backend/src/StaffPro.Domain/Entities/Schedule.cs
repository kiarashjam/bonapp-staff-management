using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class SchedulePeriod : TenantEntity
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public SchedulePeriodStatus Status { get; set; } = SchedulePeriodStatus.Draft;
    public DateTime? PublishedAt { get; set; }
    public string? PublishedBy { get; set; }
    public DateTime? LockedAt { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = [];
    public ICollection<StaffingRequirement> StaffingRequirements { get; set; } = [];
}

public class ShiftAssignment : TenantEntity
{
    public Guid SchedulePeriodId { get; set; }
    public SchedulePeriod SchedulePeriod { get; set; } = null!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid? ShiftTemplateId { get; set; }
    public ShiftTemplate? ShiftTemplate { get; set; }

    public Guid? StationId { get; set; }
    public Station? Station { get; set; }

    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int BreakDurationMinutes { get; set; } = 30;
    public bool BreakIsPaid { get; set; }

    public ShiftAssignmentStatus Status { get; set; } = ShiftAssignmentStatus.Scheduled;
    public string? Notes { get; set; }

    public double TotalHours
    {
        get
        {
            var duration = EndTime.ToTimeSpan() - StartTime.ToTimeSpan();
            if (duration < TimeSpan.Zero) duration += TimeSpan.FromHours(24);
            return duration.TotalHours;
        }
    }

    public double NetHours => TotalHours - (BreakIsPaid ? 0 : BreakDurationMinutes / 60.0);
}

public class StaffingRequirement : TenantEntity
{
    public Guid SchedulePeriodId { get; set; }
    public SchedulePeriod SchedulePeriod { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
    public Guid? ShiftTemplateId { get; set; }
    public ShiftTemplate? ShiftTemplate { get; set; }

    public int MinStaff { get; set; }
    public int MaxStaff { get; set; }
}
