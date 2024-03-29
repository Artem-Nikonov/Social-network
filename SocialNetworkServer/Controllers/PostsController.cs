﻿using Microsoft.AspNetCore.Mvc;
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
        [Route("userPost/add")]
        public async Task<IActionResult> AddUserPost([FromBody][Bind("Content")] Post post)
        {
            var isSuccess = await userPostsService.TryCreatePost(post, HttpContext);
            if(isSuccess) return Ok();
            return BadRequest();
        }
    }
}
