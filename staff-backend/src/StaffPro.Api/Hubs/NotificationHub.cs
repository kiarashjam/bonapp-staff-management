using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace StaffPro.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var orgId = Context.User?.FindFirst("organizationId")?.Value;
        if (!string.IsNullOrEmpty(orgId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"org_{orgId}");
        }

        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var orgId = Context.User?.FindFirst("organizationId")?.Value;
        if (!string.IsNullOrEmpty(orgId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"org_{orgId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
