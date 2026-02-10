using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class EmployeeRole
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Junior;
    public bool IsPrimary { get; set; }
}
