using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class Notification : TenantEntity
{
    public Guid RecipientUserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? EntityId { get; set; }
    public string? EntityType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class Announcement : TenantEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public AnnouncementTargetType TargetType { get; set; } = AnnouncementTargetType.All;
    public Guid? TargetId { get; set; } // DepartmentId, LocationId, or RoleId
    public string PostedBy { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }

    public ICollection<AnnouncementReadReceipt> ReadReceipts { get; set; } = [];
}

public class AnnouncementReadReceipt
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AnnouncementId { get; set; }
    public Announcement Announcement { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
