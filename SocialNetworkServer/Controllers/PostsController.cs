﻿using Microsoft.AspNetCore.Authorization;
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
        [Route("userPost/add")]
        public async Task<IActionResult> AddUserPost([FromBody][Bind("Content")] Post post)
        {
            var createdPost = await userPostsService.TryCreatePost(post, HttpContext);
            if(createdPost!=null) return Json(createdPost);
            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        [Route("userPost/{userId:int}")]
        public async Task<JsonResult> GetPosts(int UserId,int startPostId = 1 )
        {
            var posts= await userPostsService.GetPosts(UserId, startPostId);
            var postsData = new
            {
                Meta = new
                {
                    lastPostID = posts.LastOrDefault()?.PostId ,
                    IsLastPage = posts.Count < UserPostsService.limit
                },
                Posts = posts
            };
            return Json(postsData);
        }
    }
}