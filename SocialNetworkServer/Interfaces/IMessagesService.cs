using SocialNetworkServer.Models;

namespace SocialNetworkServer.Interfaces
{
    public interface IMessagesService
    {
        Task<MessageInfoModel> SaveMessage(MessageInfoModel messageInfo);
    }
}
