using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.Services;

namespace SocialNetworkServer.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private IGroupsService groupsService;
        private IUsersService usersService;
        public GroupsController( IGroupsService groupsService, IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
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

        [HttpGet("get")]
        public async Task<IActionResult> GetGroups(int page)
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
                    IsLastPage = groups.Count < IGroupsService.limit
                },
                Groups = groups
            };
            return Json(groupsData);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(GroupInfoModel groupInfo)
        {
            var result = await groupsService.CreateGroup(groupInfo, HttpContext.User);
            if (result == null)
                return BadRequest("Не удалось создать группу");
            return Ok(result);
        }
    }
}
