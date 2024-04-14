using SocialNetworkServer.Models;

namespace SocialNetworkServer.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<bool> SubscribeToUser(int followerId, int followeeId);
        Task<bool> UnsubscribeFromUser(int followerId, int followeeId);
        Task<List<UserInfoModel>> GetUserFollowers(int userId, int page);
        Task<List<UserInfoModel>> GetUserFollowing(int userId, int page);
        Task<List<GroupInfoModel>> GetUserGroups(int userId, int page);
    }
}
