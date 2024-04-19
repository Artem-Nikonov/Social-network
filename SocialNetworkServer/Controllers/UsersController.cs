using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.AuxiliaryClasses;
using System.Globalization;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.OptionModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetworkServer.Enums;

namespace SocialNetworkServer.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private IUsersService usersService;
        private IUserSubscriptionService userSubscriptionService;
        public UsersController(IUsersService usersService, IUserSubscriptionService userSubscriptionService)
        {
            this.usersService = usersService;
            this.userSubscriptionService = userSubscriptionService;
        }

        //получение представления списка пользователей
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //получение страницы пользователя
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> UserPage(int id)
        {
            var userInfo = await usersService.GetUserInfo(id);
            if (userInfo == null) return NotFound();
            var visitorId = usersService.GetUserId(HttpContext.User);
            var visitorIsOwner = visitorId == userInfo.UserId;
            var userPageModel = new UserPageModel(userInfo, new PageMetaData(visitorId, visitorIsOwner));
            return View(userPageModel);
        }

        //получение пользователей через пагинацию
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers([FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var users = await usersService.GetUsers(page);
            var usersData = BuildUsersData(page, users);
            return Json(usersData);
        }

        //получение представления подписок пользователя
        [Authorize]
        [HttpGet("{id:int}/subscribtions")]
        public async Task<IActionResult> UserSubscribtions(int id, [FromQuery] SubscriptionTypes filter)
        {
            var userInfo = await usersService.GetUserInfo(id);
            if (userInfo == null) return NotFound();
            switch (filter)
            {
                case SubscriptionTypes.subscriptions:
                    return View("UserSubscribtions", userInfo);
                case SubscriptionTypes.subscribers:
                    return View("UserSubscribers", userInfo);
                case SubscriptionTypes.groups:
                    return View("UserGroups", userInfo);
                default:
                    return View("UserSubscribtions", userInfo);
            }
        }

        //поучение подписчиков пользователя через пагинацию
        [Authorize]
        [HttpGet("{id:int}/subscribers/get")]
        public async Task<IActionResult> GetUserSubscribers(int id,[FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var userSubscribers = await userSubscriptionService.GetUserFollowers(id, page);
            var usersData = BuildUsersData(page, userSubscribers);
            return Json(usersData);
        }

        //поучение подписчиков пользователя через пагинацию
        [Authorize]
        [HttpGet("{id:int}/subscribtions/get")]
        public async Task<IActionResult> GetUserSubscribtions(int id, [FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var userSubscribers = await userSubscriptionService.GetUserFollowing(id, page);
            var usersData = BuildUsersData(page, userSubscribers);
            return Json(usersData);
        }

        //поучение подписчиков пользователя через пагинацию
        [Authorize]
        [HttpGet("{id:int}/groups/get")]
        public async Task<IActionResult> GetUserGroups(int id, [FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var userGroups = await userSubscriptionService.GetUserGroups(id, page);
            var groupsData = BuildGroupsData(page, userGroups);
            return Json(groupsData);
        }


        //==============
        //формирование информации о пагинации
        [NonAction]
        private object BuildUsersData(int pageId, List<UserInfoModel> users)
        {
            var usersData = new
            {
                Meta = new
                {
                    PageId = pageId,
                    IsLastPage = users.Count < PaginationConstants.UsersPerPage
                },
                Users = users
            };
            return usersData;
        }

        private object BuildGroupsData(int pageId, List<GroupInfoModel> groups)
        {
            var groupsData = new
            {
                Meta = new
                {
                    PageId = pageId,
                    IsLastPage = groups.Count < PaginationConstants.UsersPerPage
                },
                Groups = groups
            };
            return groupsData;
        }


    }
}
