using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;
using SocialNetworkServer.AuxiliaryClasses;
using System.Globalization;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.OptionModels;

namespace SocialNetworkServer.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

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

        [HttpGet("get")]
        public async Task<IActionResult> GetUsers([FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var users = await usersService.GetUsers(page);
            var usersData = new
            {
                Meta = new
                {
                    PageId = page,
                    IsLastPage = users.Count < PaginationConstants.UsersPerPage
                },
                Users = users
            };
            return Json(usersData);
        }
    }
}
