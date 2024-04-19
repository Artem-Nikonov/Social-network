using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Interfaces
{
    public interface IChatsService
    {
        Task<Chat> CreateChat(Chat chat, int creatorId);
    }
}
