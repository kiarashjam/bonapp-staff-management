namespace StaffPro.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class TenantEntity : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
}
