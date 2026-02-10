using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

public class GetNotificationsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetNotificationsQuery, (IReadOnlyList<NotificationDto> Items, int UnreadCount)>
{
    public async Task<(IReadOnlyList<NotificationDto> Items, int UnreadCount)> Handle(GetNotificationsQuery r, CancellationToken ct)
    {
        var q = db.Notifications.Where(n => n.RecipientUserId == r.UserId);
        var unreadCount = await q.CountAsync(n => !n.IsRead, ct);
        var items = await q.OrderByDescending(n => n.CreatedAt)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);
        return (mapper.Map<IReadOnlyList<NotificationDto>>(items), unreadCount);
    }
}

public class MarkNotificationReadHandler(StaffProDbContext db)
    : IRequestHandler<MarkNotificationReadCommand, Unit>
{
    public async Task<Unit> Handle(MarkNotificationReadCommand r, CancellationToken ct)
    {
        var n = await db.Notifications.FirstOrDefaultAsync(x => x.Id == r.NotificationId && x.RecipientUserId == r.UserId, ct);
        if (n != null) { n.IsRead = true; n.ReadAt = DateTime.UtcNow; await db.SaveChangesAsync(ct); }
        return Unit.Value;
    }
}

public class MarkAllNotificationsReadHandler(StaffProDbContext db)
    : IRequestHandler<MarkAllNotificationsReadCommand, Unit>
{
    public async Task<Unit> Handle(MarkAllNotificationsReadCommand r, CancellationToken ct)
    {
        var unread = await db.Notifications
            .Where(n => n.RecipientUserId == r.UserId && !n.IsRead)
            .ToListAsync(ct);
        foreach (var n in unread) { n.IsRead = true; n.ReadAt = DateTime.UtcNow; }
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class GetAnnouncementsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetAnnouncementsQuery, PagedResult<AnnouncementDto>>
{
    public async Task<PagedResult<AnnouncementDto>> Handle(GetAnnouncementsQuery r, CancellationToken ct)
    {
        var q = db.Announcements.Include(a => a.ReadReceipts)
            .Where(a => a.OrganizationId == r.OrgId && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow));

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(a => a.IsPinned).ThenByDescending(a => a.CreatedAt)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);

        var totalEmployees = await db.Employees.CountAsync(e => e.OrganizationId == r.OrgId, ct);
        var dtos = items.Select(a => new AnnouncementDto(
            a.Id, a.Title, a.Content, a.IsPinned, a.PostedBy,
            a.CreatedAt, a.ExpiresAt, a.ReadReceipts.Count, totalEmployees)).ToList();

        return new PagedResult<AnnouncementDto>(
            dtos, total, r.Page, r.PageSize, (int)Math.Ceiling(total / (double)r.PageSize));
    }
}

public class CreateAnnouncementHandler(StaffProDbContext db)
    : IRequestHandler<CreateAnnouncementCommand, AnnouncementDto>
{
    public async Task<AnnouncementDto> Handle(CreateAnnouncementCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var a = new Announcement
        {
            OrganizationId = r.OrgId, Title = req.Title, Content = req.Content,
            IsPinned = req.IsPinned, PostedBy = r.PostedBy, ExpiresAt = req.ExpiresAt,
            TargetType = Enum.TryParse<AnnouncementTargetType>(req.TargetType, out var tt) ? tt : AnnouncementTargetType.All,
            TargetId = req.TargetId
        };
        db.Announcements.Add(a);
        await db.SaveChangesAsync(ct);
        var totalEmployees = await db.Employees.CountAsync(e => e.OrganizationId == r.OrgId, ct);
        return new AnnouncementDto(a.Id, a.Title, a.Content, a.IsPinned, a.PostedBy, a.CreatedAt, a.ExpiresAt, 0, totalEmployees);
    }
}

public class MarkAnnouncementReadHandler(StaffProDbContext db)
    : IRequestHandler<MarkAnnouncementReadCommand, Unit>
{
    public async Task<Unit> Handle(MarkAnnouncementReadCommand r, CancellationToken ct)
    {
        var exists = await db.AnnouncementReadReceipts
            .AnyAsync(rr => rr.AnnouncementId == r.AnnouncementId && rr.EmployeeId == r.EmployeeId, ct);
        if (!exists)
        {
            db.AnnouncementReadReceipts.Add(new AnnouncementReadReceipt
            {
                AnnouncementId = r.AnnouncementId, EmployeeId = r.EmployeeId
            });
            await db.SaveChangesAsync(ct);
        }
        return Unit.Value;
    }
}
