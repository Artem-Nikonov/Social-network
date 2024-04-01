using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class UserPostsService
    {
        private SocialNetworkDBContext dbContext;
        private UserService userService;
        public static int limit { get; private set; } = 5;

        public UserPostsService(SocialNetworkDBContext dbContext, UserService userService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }

        public async Task<PostInfoModel> CreatePost(Post post, ClaimsPrincipal user)
        {
            if (post.Content == null || post.Content.Length < 2)
                throw new ArgumentException("Длина поста должна быть не менее 2-х символов");
            var userId = userService.GetUserId(user);
            post.UserId = userId;
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            PostInfoModel postInfo = post;
            return postInfo;
        }

        public async Task<PostInfoModel?> DeletePost(int postId, ClaimsPrincipal user)
        {
            var userId = userService.GetUserId(user);
            var post = await dbContext.Posts.FindAsync(postId);
            if (post == null || post.UserId != userId) return null;
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
            PostInfoModel postInfo = post;
            return postInfo;
        }

        public async Task<List<PostInfoModel>> GetPosts(int userId, int startPostId)
        {
            IQueryable<Post> query = dbContext.Posts.OrderByDescending(p => p.PostId)
                                          .Where(p => p.UserId == userId && p.GroupId == null);

            if (startPostId > 0)
                query = query.Where(p => p.PostId <= startPostId);

            var posts = await query.Take(limit)
                .Select(post => new PostInfoModel
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    GroupId = post.GroupId,
                    Content = post.Content,
                    CreationDate = post.CreationDate.GetSpecialFormat()
                }).AsNoTracking().ToListAsync();
            return posts;
        }
    }
}
