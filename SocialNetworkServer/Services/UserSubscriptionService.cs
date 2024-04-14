using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Services
{
    public class UserSubscriptionService : IUserSubscriptionService, ISubscribeChecker
    {
        private SocialNetworkDBContext dbContext;

        public UserSubscriptionService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> SubscribeToUser(int followerId, int followeeId)
        {
            if(followerId == followeeId) return false;
            var subscription = await TryFindSubscription(followerId, followeeId);
            if (subscription != null) return false;
            subscription = new UserSubscription
            {
                SubscriberId = followerId,
                SubscribedToUserId = followeeId,
            };
            await dbContext.UserSubscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsubscribeFromUser(int followerId, int followeeId)
        {
            if (followerId == followeeId) return false;
            var subscription = await TryFindSubscription(followerId, followeeId);
            if (subscription == null) return false;
            dbContext.UserSubscriptions.Remove(subscription);
            await dbContext.SaveChangesAsync();
            return true;
        }

        //получение подписчиков пользователя
        public async Task<List<UserInfoModel>> GetUserFollowers(int userId, int page)
        {
            if (page <= 0) page = 1;
            var users = await dbContext.UserSubscriptions.Include(us=>us.Subscriber)
                .Where(us=>us.SubscribedToUserId == userId)
                .Skip((page - 1) * PaginationConstants.UsersPerPage)
                .Take(PaginationConstants.UsersPerPage).Select(us => new UserInfoModel
                {
                    UserId = us.SubscriberId,
                    UserName = us.Subscriber.UserName,
                    UserSurname = us.Subscriber.UserSurname
                }).AsNoTracking().ToListAsync();
            return users;
        }

        //получение подписок пользователя
        public async Task<List<UserInfoModel>> GetUserFollowing(int userId, int page)
        {
            if (page <= 0) page = 1;
            var users = await dbContext.UserSubscriptions.Include(us => us.SubscribedToUser)
                .Where(us => us.SubscriberId == userId)
                .Skip((page - 1) * PaginationConstants.UsersPerPage)
                .Take(PaginationConstants.UsersPerPage).Select(us => new UserInfoModel
                {
                    UserId = us.SubscribedToUserId,
                    UserName = us.SubscribedToUser.UserName,
                    UserSurname = us.SubscribedToUser.UserSurname
                }).AsNoTracking().ToListAsync();
            return users;
        }

        //получение групп на которые подписан пользователь
        public async Task<List<GroupInfoModel>> GetUserGroups(int userId, int page)
        {
            if (page <= 0) page = 1;
            var groups = await dbContext.GroupSubscriptions.Include(gs => gs.SubscribedToGroup)
                .Where(gs => gs.SubscriberId == userId)
                .Skip((page - 1) * PaginationConstants.GroupsPerPage)
                .Take(PaginationConstants.GroupsPerPage).Select(gs => new GroupInfoModel
                {
                    GroupId = gs.SubscribedToGroupId,
                    GroupName = gs.SubscribedToGroup.GroupName,
                }).AsNoTracking().ToListAsync();
            return groups;
        }

        
        public async Task<bool> UserIsSubscribeToGroup(int userId, int GroupId)
        {
            var subscription = await dbContext.GroupSubscriptions.AsNoTracking()
                .FirstOrDefaultAsync(s =>s.SubscriberId == userId &&
                s.SubscribedToGroupId==GroupId);
            return subscription != null;
        }

        private async Task<UserSubscription?> TryFindSubscription(int followerId, int followeeId)
        {
            var subscription = await dbContext.UserSubscriptions
               .FirstOrDefaultAsync(s => s.SubscriberId == followerId &&
               s.SubscribedToUserId == followeeId);
            return subscription;
        }
    }
}
