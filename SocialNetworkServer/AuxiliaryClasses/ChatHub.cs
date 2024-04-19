using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialNetworkServer.AuxiliaryClasses
{
    //[Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string chatId, string message, string userName)
        {
            //var uid = Context.User.Identity.Name;
            //Console.WriteLine(uid);
            await Clients.All.SendAsync("Receive", message, userName);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
