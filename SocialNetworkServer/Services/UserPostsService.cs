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
        public static int limit { get; private set; } = 5;
        public UserPostsService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<PostInfoModel?> TryCreatePost(Post post, HttpContext context)
        {
            var strUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (strUserId == null || !int.TryParse(strUserId, out int userId)) return null;
            post.UserId = userId;
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            var postInfo = post;
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
