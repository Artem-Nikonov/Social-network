using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Security.Claims;

namespace SocialNetworkServer.Services
{
    public class UserPostsService
    {
        private SocialNetworkDBContext dbContext;
        private IUsersService userService;

        public UserPostsService(SocialNetworkDBContext dbContext, IUsersService userService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }

        //создание поста
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

        //удаление поста
        public async Task<PostInfoModel?> DeletePost(int postId, ClaimsPrincipal user)
        {
            var userId = userService.GetUserId(user);
            var post = await dbContext.Posts.FindAsync(postId);
            if (post == null || post.UserId != userId || post.IsDeleted) return null;
            post.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            PostInfoModel postInfo = post;
            return postInfo;
        }

        //получение постов через пагинацию
        public async Task<List<PostInfoModel>> GetPosts(int userId, int startPostId)
        {
            IQueryable<Post> query = dbContext.Posts.OrderByDescending(p => p.PostId)
                                          .Where(p => p.UserId == userId && p.GroupId == null && !p.IsDeleted);

            if (startPostId > 0)
                query = query.Where(p => p.PostId <= startPostId);

            var posts = await query.Take(PaginationConstants.PostsPerPage)
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
