using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class LeaveType : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsPaid { get; set; } = true;
    public bool RequiresDocument { get; set; }
    public int? MaxDaysPerYear { get; set; }
    public decimal? AccrualRatePerMonth { get; set; }
    public int MaxCarryOverDays { get; set; }
    public string Color { get; set; } = "#F59E0B";

    public ICollection<TimeOffRequest> TimeOffRequests { get; set; } = [];
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = [];
}

public class TimeOffRequest : TenantEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly? StartTime { get; set; } // For partial days
    public TimeOnly? EndTime { get; set; }
    public string? Reason { get; set; }
    public string? AttachmentUrl { get; set; }

    public TimeOffRequestStatus Status { get; set; } = TimeOffRequestStatus.Pending;
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? DenialReason { get; set; }

    public int TotalDays => EndDate.DayNumber - StartDate.DayNumber + 1;
}

public class LeaveBalance : TenantEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;

    public int Year { get; set; }
    public decimal Entitled { get; set; }
    public decimal Used { get; set; }
    public decimal CarriedOver { get; set; }
    public decimal Adjustment { get; set; }

    public decimal Remaining => Entitled + CarriedOver + Adjustment - Used;
}
