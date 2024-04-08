using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class GroupPostsService : IPostsService
    {
        public Task<PostInfoModel> CreatePost(Post post, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<PostInfoModel?> DeletePost(int postId, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<List<PostInfoModel>> GetPosts(int pageId, int startPostId)
        {
            throw new NotImplementedException();
        }
    }
}
