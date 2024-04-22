﻿using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class ChatsService : IChatsService, IChatParticipantChecker
    {
        private SocialNetworkDBContext dbContext;

        public ChatsService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Получение информации о конкретном чате
        public async Task<ChatInfoModel?> GetChatInfo(int chatId)
        {
            var chat = await dbContext.Chats.FindAsync(chatId);
            if (chat == null) return null;
            return chat;
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
        public async Task<bool> AddUserInChat(int chatId, int UserId)
        {
            var chatParticipant = new ChatParticipants
            {
                ChatId = chatId,
                UserId = UserId
            };
            try
            {
                await dbContext.ChatParticipants.AddAsync(chatParticipant);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch {  return false; }
            
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
            .AsNoTracking().ToListAsync();
            return chats;
        }

        public async Task<bool> UserIsAChatParcipant(int userId, int chatId)
        {
            var userIsParticipant = await dbContext.ChatParticipants
                .FirstOrDefaultAsync(cp=>cp.ChatId == chatId && cp.UserId==userId);
            return userIsParticipant != null;
        }
    }

    class ChatException:Exception
    {
        public ChatException(string message): base(message) { }
    }
}
