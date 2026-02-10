using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class Employee : TenantEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? NationalId { get; set; } // Encrypted
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }

    // Relationships
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }
    public Guid? AppUserId { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // Navigation
    public Contract? ActiveContract { get; set; }
    public ICollection<Contract> Contracts { get; set; } = [];
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = [];
    public ICollection<RecurringAvailability> RecurringAvailabilities { get; set; } = [];
    public ICollection<AvailabilityOverride> AvailabilityOverrides { get; set; } = [];
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = [];
    public ICollection<TimeOffRequest> TimeOffRequests { get; set; } = [];
    public ICollection<ClockEntry> ClockEntries { get; set; } = [];
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = [];
}
