using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Enums;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.Services;

namespace SocialNetworkServer.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private IGroupsService groupsService;
        private IUsersService usersService;
        private IGroupSubscriptionService groupSubscriptionService;
        public GroupsController( IGroupsService groupsService, IUsersService usersService, IGroupSubscriptionService groupSubscriptionService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
            this.groupSubscriptionService = groupSubscriptionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet("create")]
        public IActionResult CreateGroup()
        {
            return View();
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GroupPage(int id)
        {
            var groupInfo = await groupsService.GetGroupInfo(id);
            if (groupInfo == null) return NotFound();
            var visitorId = usersService.GetUserId(HttpContext.User);
            var visitorIsOwner = visitorId == groupInfo.CreatorId;
            var groupPageModel = new GroupPageModel(groupInfo, new PageMetaData(visitorId, visitorIsOwner));
            return View(groupPageModel);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetGroups([FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var groups = await groupsService.GetGroups(page);

            var groupsData = new
            {
                Meta = new
                {
                    PageId = page,
                    IsLastPage = groups.Count < PaginationConstants.GroupsPerPage
                },
                Groups = groups
            };
            return Json(groupsData);
        }

        [HttpGet("{id:int}/subscribers")]
        public async Task<IActionResult> GroupSubscribers(int id)
        {
            var groupInfo = await groupsService.GetGroupInfo(id);
            if (groupInfo == null) return NotFound();
            return View(groupInfo);
        }

        [HttpGet("{id:int}/subscribers/list")]
        public async Task<IActionResult> GetGroupSubscribers(int id, [FromQuery] int page)
        {
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var users = await groupSubscriptionService.GetGroupSubscribers(id, page);

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

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(GroupInfoModel groupInfo)
        {
            var result = await groupsService.CreateGroup(groupInfo, HttpContext.User);
            if (result == null)
                return BadRequest("Не удалось создать группу");
            return Redirect("/groups");
        }
    }
}
