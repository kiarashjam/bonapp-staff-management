namespace StaffPro.Domain.Entities;

public class Department : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";

    // Navigation
    public ICollection<Role> Roles { get; set; } = [];
}
