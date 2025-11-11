using Microsoft.AspNetCore.SignalR;

namespace BLL.Hubs
{
    public class NotificationsHub : Hub
    {
        public Task JoinGroup(string group) => Groups.AddToGroupAsync(Context.ConnectionId, group);
        public Task LeaveGroup(string group) => Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }
}
