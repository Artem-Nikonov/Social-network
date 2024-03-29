using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class UserPostsService
    {
        private SocialNetworkDBContext dbContext;
        public int partSize { get; private set; } = 5;
        public UserPostsService(SocialNetworkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> TryCreatePost(Post post, HttpContext context)
        {
            var strUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (strUserId == null || !int.TryParse(strUserId, out int userId)) return false;
            post.UserId = userId;
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetPosts(int userId,int partIndex)
        {
            var posts = await dbContext.Posts.OrderByDescending(p => p.PostId)
                .Where(p => p.UserId == userId && p.GroupId == null)
                .Skip((partIndex - 1) * partSize).Take(partSize).Select(p =>
                new
                {
                    PostId = p.PostId,
                    UserId = userId,
                    Content = p.Content,
                    CreationDate = p.CreationDate.RemoveSeconds()
                }).Cast<object>().ToListAsync();
            return posts;
        }
    }
}
