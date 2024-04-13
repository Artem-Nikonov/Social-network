using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Interfaces
{
    public interface IPostsService
    {
        Task<PostInfoModel> CreatePost(Post post, ClaimsPrincipal user);
        Task<bool> DeletePost(int postId, ClaimsPrincipal user);
        Task<List<PostInfoModel>> GetPosts(int pageId, int startPostId);
    }
}
