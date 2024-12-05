using Microsoft.AspNetCore.SignalR;

namespace M_Sinca_Teodora_Ioana_Lab2.Hubs
{
    public class NotificationHub: Hub
    {
        private readonly static PresenceTracker presenceTracker = new PresenceTracker();
        public override async Task OnConnectedAsync()
        {
            var result = await presenceTracker.ConnectionOpened(Context.User.Identity.Name);
            if (result.UserJoined)
            {
                await Clients.All.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} joined");
            }
            var currentUsers = await presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("newMessage", "system", $"Currently online:\n{string.Join("\n",currentUsers)}");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var result = await presenceTracker.ConnectionClosed(Context.User.Identity.Name);
            if (result.UserLeft)
            {
                await Clients.All.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} left");
            }
            await base.OnDisconnectedAsync(exception);
        }

    }
}
