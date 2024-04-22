using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialNetworkServer.AuxiliaryClasses
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Enter( string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //await Clients.All.SendAsync("Notify", $"{username} вошел в чат в группу {groupName}");
        }

        public async Task Send(string message, string groupName)
        {
            var userName = Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("Receive", message, userName);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} вышел");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
