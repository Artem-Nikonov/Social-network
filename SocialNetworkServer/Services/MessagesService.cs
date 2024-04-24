using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
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
            await dbContext.Entry(message).Reference(message=>message.User).Query().LoadAsync();
            return (MessageInfoModel)message;
        }

        public async Task<List<MessageInfoModel>> GetMessages(int chatId, int startMessageId)
        {
            IQueryable<Message> query = dbContext.Messages.Include(m=>m.User).OrderByDescending(m=>m.MessageId)
                                          .Where(m => m.ChatId==chatId && !m.IsDeleted);

            if (startMessageId > 0)
                query = query.Where(m => m.MessageId <= startMessageId);

            var messages = await query.Take(PaginationConstants.MessagesPerPage)
                .Select(message => (MessageInfoModel)message).AsNoTracking().ToListAsync();
            return messages;
        }
    }
}
