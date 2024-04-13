using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<bool> SubscribeToUser(int followerId, int followeeId);
        Task<bool> UnsubscribeFromUser(int followerId, int followeeId);
        Task<List<UserInfo>> GetUserFollowers(int userId, int page);
        Task<List<UserInfo>> GetUserFollowing(int userId, int page);
    }
}
