using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkServer.Models;
using System.Diagnostics;

namespace SocialNetworkServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/PR")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [Route("/SEC")]
        public IActionResult Secret()
        {
            return Content("секретная страница.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("httpError/{statusCode}")]
        public IActionResult HttpError(int statusCode)
        {
            return View(statusCode);
        }
    }
}