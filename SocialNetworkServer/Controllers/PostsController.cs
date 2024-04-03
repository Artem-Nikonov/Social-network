using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Services;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Controllers
{
    public class PostsController : Controller
    {
        private UserPostsService userPostsService;
        public PostsController(UserPostsService userPostsService)
        {
            this.userPostsService = userPostsService;
        }

        [Authorize]
        [HttpPost]
        [Route("userPosts/create")]
        public async Task<IActionResult> CreateUserPost([FromBody][Bind("Content")] Post post)
        {
            try
            {
                var createdPost = await userPostsService.CreatePost(post, HttpContext.User);
                return Json(createdPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("userPosts/{userId:int}")]
        public async Task<JsonResult> GetPosts(int UserId, int startPostId)
        {
            var posts= await userPostsService.GetPosts(UserId, startPostId);
            var postsData = new
            {
                Meta = new
                {
                    LastPostId = posts.LastOrDefault()?.PostId,
                    IsLastPage = posts.Count < UserPostsService.limit
                },
                Posts = posts
            };
            return Json(postsData);
        }

        [Authorize]
        [HttpPatch]
        [Route("userPosts/delete/{postId:int}")]
        public async Task<IActionResult> CreateUserPost(int postId)
        {
            var deletedPost = await userPostsService.DeletePost(postId, HttpContext.User);
            if (deletedPost != null) return Ok();
            return BadRequest("Не удалось удалить пост.");
        }
    }
}
