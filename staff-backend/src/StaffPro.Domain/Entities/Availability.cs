using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class RecurringAvailability : TenantEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public AvailabilityType Type { get; set; } = AvailabilityType.Available;
}

public class AvailabilityOverride : TenantEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateOnly Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public AvailabilityType Type { get; set; }
    public string? Note { get; set; }
}
