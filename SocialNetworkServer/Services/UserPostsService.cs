using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class UserPostsService
    {
        private SocialNetworkDBContext dbContext;
        public UserPostsService(SocialNetworkDBContext dBContext)
        {
            this.dbContext = dBContext;
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
    }
}
