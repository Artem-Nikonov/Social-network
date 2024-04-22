using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Interfaces
{
    public interface IChatsService
    {
        Task<ChatInfoModel> CreateChat(ChatInfoModel chatInfo, int creatorId);
        Task<ChatInfoModel?> GetChatInfo(int chatId);
        Task<bool> AddUserInChat(int chatId, int UserId);
        Task<List<UserInfoModel>> GetChatUsers(int chatId, int page);
        Task<List<ChatInfoModel>> GetUserChats(int userId, int page);
    }
}
