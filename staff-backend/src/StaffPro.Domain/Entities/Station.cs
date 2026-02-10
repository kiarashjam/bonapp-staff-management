namespace StaffPro.Domain.Entities;

public class Station : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int MaxCapacity { get; set; } = 5;
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }

    // Navigation
    public ICollection<StationRole> StationRoles { get; set; } = [];
}

public class StationRole
{
    public Guid StationId { get; set; }
    public Station Station { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
