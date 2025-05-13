using Announcements.UI.Models;
using Announcements.UI.Models.DTOs;
using Announcements.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Announcements.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnnouncementsService _api;

        public HomeController(ILogger<HomeController> logger, IAnnouncementsService api)
            => (_logger, _api) = (logger, api);

        public async Task<IActionResult> Index()
        {
            var model = new AnnouncementsDisplayModel
            {
                List = await _api.ReadAll()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
