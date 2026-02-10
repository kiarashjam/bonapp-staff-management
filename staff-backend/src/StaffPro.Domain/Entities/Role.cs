namespace StaffPro.Domain.Entities;

public class Role : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultHourlyRate { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // Navigation
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = [];
    public ICollection<StationRole> StationRoles { get; set; } = [];
    public ICollection<StaffingRequirement> StaffingRequirements { get; set; } = [];
}
