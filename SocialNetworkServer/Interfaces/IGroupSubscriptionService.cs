using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Interfaces
{
    public interface IGroupSubscriptionService
    {
        Task<bool> SubscribeToGroup(int userId, int groupId);
        Task<bool> UnsubscribeFromGroup(int userId, int groupId);
        Task<List<UserInfo>> GetGroupSubscribers(int groupId);
    }
}
