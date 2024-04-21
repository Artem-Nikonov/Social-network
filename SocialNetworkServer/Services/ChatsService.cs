using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class ChatsService : IChatsService
    {
        private SocialNetworkDBContext dbContext;

        public ChatsService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //создание чата
        public async Task<ChatInfoModel> CreateChat(ChatInfoModel chatInfo, int creatorId)
        {
            if(chatInfo.ChatName.Length<1)
                throw new ChatException("Длина названия чата должна быть не менее 2-х символов");
            var chat = new Chat()
            {
                ChatName = chatInfo.ChatName,
                CreatorId = creatorId
            };
            await dbContext.Chats.AddAsync(chat);
            await dbContext.SaveChangesAsync();
            var ChatParticipant = new ChatParticipants
            {
                ChatId = chat.ChatId,
                UserId = creatorId
            };
            await dbContext.ChatParticipants.AddAsync(ChatParticipant);
            await dbContext.SaveChangesAsync();
            //пользовательское преобразование
            return chat;
        }

        //добавление пользователя в чат
        public Task<bool> AddUserInChat(int chatId, int UserId)
        {
            throw new NotImplementedException();
        }

        //полцчение пользователей чата
        public async Task<List<UserInfoModel>> GetChatUsers(int chatId, int page)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChatInfoModel>> GetUserChats(int userId, int page)
        {
            if (page <= 0) page = 1;
            var chats = await dbContext.ChatParticipants
            .Where(cp => cp.UserId == userId)
            .Skip((page - 1) * PaginationConstants.ChatsPerPage)
            .Take(PaginationConstants.ChatsPerPage)
            .Select(cp => (ChatInfoModel)cp.Chat)
            .ToListAsync();
            return chats;
        }
    }

    class ChatException:Exception
    {
        public ChatException(string message): base(message) { }
    }
}
