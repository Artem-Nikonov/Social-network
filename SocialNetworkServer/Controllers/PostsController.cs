﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Enums;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.Services;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Controllers
{
    public class PostsController : Controller
    {
        private IPostsService userPostsService;
        private IPostsService groupPostsService;
        public PostsController(IEnumerable<IPostsService> postsSerwices)
        {
            userPostsService = postsSerwices.OfType<UserPostsService>().FirstOrDefault()!;
            groupPostsService = postsSerwices.OfType<GroupPostsService>().FirstOrDefault()!;
        }

        [Authorize]
        [HttpGet("pagePosts/{pageId:int}")]
        public async Task<IActionResult> GetUserPosts(int pageId, [FromQuery] PageTypes pageType, [FromQuery] int startPostId)
        {
            if (!Enum.IsDefined(typeof(PageTypes), pageType))
                return BadRequest("Некорректный тип страницы");

            List<PostInfoModel> posts = null;
            switch (pageType)
            {
                case PageTypes.userPage:
                    posts = await userPostsService.GetPosts(pageId, startPostId);
                    break;
                case PageTypes.group:
                    posts = await groupPostsService.GetPosts(pageId, startPostId);
                    break;
            }

            if (posts == null)
                return BadRequest("Некорректный запрос");

            var postsData = new
            {
                Meta = new
                {
                    LastPostId = posts.LastOrDefault()?.PostId,
                    IsLastPage = posts.Count < PaginationConstants.PostsPerPage
                },
                Posts = posts
            };

            return Json(postsData);
        }

        [Authorize]
        [HttpPost("posts/create")]
        public async Task<IActionResult> CreateUserPost([FromBody][Bind("Content", "GroupId")] Post post)
        {
            try
            {
                PostInfoModel createdPost;

                if(post.GroupId == null)
                    createdPost = await userPostsService.CreatePost(post, HttpContext.User);
                else
                    createdPost = await groupPostsService.CreatePost(post,HttpContext.User);
                return Json(createdPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GroupPostsException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("pagePosts/delete/{postId:int}")]
        public async Task<IActionResult> DeleteUserPost(int postId, [FromQuery] PageTypes pageType)
        {
            if (!Enum.IsDefined(typeof(PageTypes), pageType))
                return BadRequest("Некорректный тип страницы");
            var deletoonIsSuccessfull = false;
            switch (pageType)
            {
                case PageTypes.userPage:
                    deletoonIsSuccessfull = await userPostsService.DeletePost(postId, HttpContext.User);
                    break;
                case PageTypes.group:
                    deletoonIsSuccessfull = await groupPostsService.DeletePost(postId,HttpContext.User);
                    break;
            }
            if (deletoonIsSuccessfull) return Ok();
            return BadRequest("Не удалось удалить пост.");
        }
    }
}
