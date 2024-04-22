using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class MessagesService : IMessagesService
    {
        private SocialNetworkDBContext dbContext;

        public MessagesService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<MessageInfoModel> SaveMessage(MessageInfoModel messageInfo)
        {
            var message = new Message()
            {
                UserId = messageInfo.UserId,
                ChatId = messageInfo.ChatId,
                Content = messageInfo.Content,
            };
            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();
            return message;
        }
    }
}
