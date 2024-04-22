using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using SocialNetworkServer.Enums;
using System.Security.Claims;
using SocialNetworkServer.OptionModels;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkServer.Services
{
    public class GroupPostsService : IPostsService
    {
        private SocialNetworkDBContext dbContext;
        private IGroupsService groupsService;
        private IUsersService usersService;
        private ISubscribeChecker subscribeChecker;

        public GroupPostsService (SocialNetworkDBContext dbContext, IGroupsService groupsService,
            IUsersService usersService, ISubscribeChecker subscribeChecker)
        {
            this.dbContext = dbContext;
            this.groupsService = groupsService;
            this.usersService = usersService;
            this.subscribeChecker = subscribeChecker;
        }

        public async Task<PostInfoModel?> CreatePost(Post post, ClaimsPrincipal user)
        {
            if (post.Content == null || post.Content.Length < 2)
                throw new ArgumentException("Длина поста должна быть не менее 2-х символов");
            
            if (!post.GroupId.HasValue) return null;

            var groupId = post.GroupId.Value;
            var userId = usersService.GetUserId(user);
            var groupInfo = await groupsService.GetGroupInfo(groupId);

            if (groupInfo == null) return null;

            if (!await CanUserPostToGroup(userId, groupInfo))
                throw new GroupPostsException("Вы не можете опубликовать пост");

            post.UserId = userId;
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            var postInfo = post;
            return postInfo;
        }

        public async Task<bool> DeletePost(int postId, ClaimsPrincipal user)
        {
            var userId = usersService.GetUserId(user);
            var post = await dbContext.Posts.Include(p=>p.Group)
                .ThenInclude(g=>g.Creator)
                .FirstOrDefaultAsync(p => p.PostId == postId);
            if (post == null ||  post.IsDeleted) return false;
            if(post.UserId == userId || post.Group.CreatorId == userId)
            {
                post.IsDeleted = true;
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<PostInfoModel>> GetPosts(int pageId, int startPostId)
        {
            IQueryable<Post> query = dbContext.Posts.OrderByDescending(p => p.PostId)
                                          .Where(p => p.GroupId == pageId && !p.IsDeleted);

            if (startPostId > 0)
                query = query.Where(p => p.PostId <= startPostId);

            var posts = await query.Take(PaginationConstants.PostsPerPage)
                .Select(post => (PostInfoModel) post).AsNoTracking().ToListAsync();
            return posts;
        }

        private async Task<bool> CanUserPostToGroup(int userId, GroupInfoModel groupInfo)
        {
            switch (groupInfo.PostPermissions)
            {
                case PostPermissions.AdminOnly:
                    return userId == groupInfo.CreatorId;
                case PostPermissions.SubscribersOnly:
                    return (await subscribeChecker.UserIsSubscribeToGroup(userId, groupInfo.GroupId) || userId == groupInfo.CreatorId);
                case PostPermissions.Everyone:
                    return true;
                default:
                    return false;
            }
        }
    }

    public class GroupPostsException: Exception
    {
        public GroupPostsException(string message) : base(message) { }
    }

}
