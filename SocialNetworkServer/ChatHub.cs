using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using System.Security.Claims;

namespace SocialNetworkServer
{
    [Authorize]
    public class ChatHub : Hub
    {
        private  IChatParticipantChecker chatParticipantChecker;
        private IMessagesService messagesService;

        public ChatHub(IChatParticipantChecker chatParticipantChecker, IMessagesService messagesService)
        {
            this.chatParticipantChecker = chatParticipantChecker;
            this.messagesService = messagesService;
        }

        public async Task Enter(int chatId)
        {
            int userId = GetUserIdFromContext();
            if (userId == 0 || !IsChatIdValid(chatId)) return;

            if (!await chatParticipantChecker.UserIsAChatParcipant(userId, chatId))
                return;

            string groupName = GetGroupName(chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task Send(string message, int chatId)
        {
            int userId = GetUserIdFromContext();
            if (userId == 0 || !IsChatIdValid(chatId) || string.IsNullOrEmpty(message)) return;
            string groupName = GetGroupName(chatId);
            string userFullName = Context.User?.Identity?.Name ?? "Unknown";
            var messageInfo = await messagesService.SaveMessage(new MessageInfoModel(chatId, userId, message));
            var userInfo = new UserInfoModel(GetUserIdFromContext(), userFullName);
            await Clients.Group(groupName).SendAsync("Receive", messageInfo, userInfo);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        private int GetUserIdFromContext()
        {
            if (int.TryParse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        private bool IsChatIdValid(int chatId) => chatId > 0;

        private string GetGroupName(int chatId) => $"ch{chatId}";
    }
}
