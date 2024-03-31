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
        
        [HttpPost]
        [Authorize]
        [Route("userPosts/create")]
        public async Task<IActionResult> CreateUserPost([FromBody][Bind("Content")] Post post)
        {
            var createdPost = await userPostsService.TryCreatePost(post, HttpContext);
            if(createdPost!=null) return Json(createdPost);
            return BadRequest();
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
    }
}
