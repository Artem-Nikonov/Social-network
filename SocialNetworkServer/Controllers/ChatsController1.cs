using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetworkServer.Controllers
{
    [Authorize]
    [Route("myChats")]
    public class ChatsController : Controller
    {
        //получение страницы с чатами пользователя
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
