namespace StaffPro.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? LogoUrl { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string Timezone { get; set; } = "Europe/Zurich";
    public string Currency { get; set; } = "CHF";
    public string DefaultLanguage { get; set; } = "en";

    // Scheduling Rules
    public int MinRestHoursBetweenShifts { get; set; } = 11;
    public int MaxConsecutiveWorkDays { get; set; } = 6;
    public int MaxHoursPerWeek { get; set; } = 48;
    public int MaxHoursPerDay { get; set; } = 10;
    public int BreakAfterMinutes { get; set; } = 360; // 6 hours
    public int BreakDurationMinutes { get; set; } = 30;
    public int ClockInGraceMinutes { get; set; } = 5;
    public int ClockInLateThresholdMinutes { get; set; } = 15;
    public int ClockRoundingMinutes { get; set; } = 5;
    public int OvertimeWeeklyThreshold { get; set; } = 40;
    public int OvertimeDailyThreshold { get; set; } = 8;
    public decimal OvertimeMultiplier { get; set; } = 1.5m;

    // Navigation
    public ICollection<Location> Locations { get; set; } = [];
    public ICollection<Employee> Employees { get; set; } = [];
    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<Station> Stations { get; set; } = [];
    public ICollection<Department> Departments { get; set; } = [];
    public ICollection<ShiftTemplate> ShiftTemplates { get; set; } = [];
    public ICollection<LeaveType> LeaveTypes { get; set; } = [];
}
