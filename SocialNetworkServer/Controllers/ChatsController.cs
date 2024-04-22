using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using SocialNetworkServer.AuxiliaryClasses;
using SocialNetworkServer.Enums;
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
        private IChatParticipantChecker chatParticipantChecker;
        private IUsersService userService;
        private IPaginator paginator;

        public ChatsController(SocialNetworkDBContext dbContext, IChatsService chatsService,
            IChatParticipantChecker chatParticipantChecker, IUsersService userService, IPaginator paginator)
        {
            this.dbContext = dbContext;
            this.chatsService = chatsService;
            this.chatParticipantChecker = chatParticipantChecker;
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
        public async Task<IActionResult> GoToChat(int chatId)
        {
            var userId = userService.GetUserId(HttpContext.User);
            var userIsChatParcipant = await chatParticipantChecker.UserIsAChatParcipant(userId, chatId);
            if(!userIsChatParcipant) return Forbid();
            var chatInfo = await chatsService.GetChatInfo(chatId);
            return View("Chat", chatInfo);
        }

        [HttpGet("{chatId:int}/users")]
        public async Task<IActionResult> GetChatUsers(int chatId, [FromQuery] int page)
        {
            var userId = userService.GetUserId(HttpContext.User);
            if (page <= 0)
            {
                return BadRequest("Номер страницы должен быть больше 0.");
            }
            var chatUsers = await chatsService.GetChatUsers(chatId, page);
            var usersData = paginator.BuildPaginationDataFromPageId(chatUsers, page, PaginationConstants.UsersPerPage);
            return Json(usersData);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetUserChats([FromQuery] int page)
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

        [HttpPost("{chatId:int}")]
        public async Task<IActionResult> AddUserToChat(int chatId,[FromQuery] ChatActions act, [FromQuery] int userId)
        {
            var inviteeId = userService.GetUserId(HttpContext.User);
            var userIsChatParcipant = await chatParticipantChecker.UserIsAChatParcipant(inviteeId, chatId);
            Console.WriteLine(userIsChatParcipant);
            if (!userIsChatParcipant) return Forbid();
            bool isSuccess;

            switch (act)
            {
                case ChatActions.addUser:
                    isSuccess = await chatsService.AddUserInChat(chatId, userId);
                    break;
                default:
                   isSuccess= await chatsService.AddUserInChat(chatId, userId);
                    break;
            }
            if (isSuccess) return Ok();
            return BadRequest();
        }

    }
}
