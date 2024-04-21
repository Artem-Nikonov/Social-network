using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Models;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.Services;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.Controllers
{
    [Authorize]
    [Route("myChats")]
    public class ChatsController : Controller
    {
        private SocialNetworkDBContext dbContext;
        private IChatsService chatsService;
        private IUsersService userService;
        private IPaginator paginator;

        public ChatsController(SocialNetworkDBContext dbContext, IChatsService chatsService, IUsersService userService, IPaginator paginator)
        {
            this.dbContext = dbContext;
            this.chatsService = chatsService;
            this.userService = userService;
            this.paginator = paginator;
        }

        //получение страницы с чатами пользователя
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{chatId:int}")]
        public IActionResult GoToChat(int chatId)
        {
            return View("Chat", chatId);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetUserSubscribtions([FromQuery] int page)
        {
            var userId = userService.GetUserId(HttpContext.User);
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var userChats = await chatsService.GetUserChats(userId, page);
            var chatsData = paginator.BuildPaginationDataFromPageId(userChats, page, PaginationConstants.ChatsPerPage);
            return Json(chatsData);
        }

        //создание чата
        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] ChatInfoModel chatInfo)
        {
            if (chatInfo == null || chatInfo.ChatName == null) return BadRequest();
            var creatorId = userService.GetUserId(HttpContext.User);
            try
            {
                var result = await chatsService.CreateChat(chatInfo, creatorId);
                return Json(result);
            }
            catch (ChatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
