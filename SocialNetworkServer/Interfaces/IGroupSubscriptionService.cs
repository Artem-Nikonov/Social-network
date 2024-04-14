using SocialNetworkServer.Models;

namespace SocialNetworkServer.Interfaces
{
    public interface IGroupSubscriptionService
    {
        Task<bool> SubscribeToGroup(int userId, int groupId);
        Task<bool> UnsubscribeFromGroup(int userId, int groupId);
        Task<List<UserInfoModel>> GetGroupSubscribers(int groupId, int page);
    }
}
