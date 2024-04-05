using BabunBank.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;

namespace BabunBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LandingPageService _landingPageService;
        public HomeController(ILogger<HomeController> logger, LandingPageService landingPageService)
        {
            _logger = logger;
            _landingPageService = landingPageService;
        }

        public async Task<IActionResult> Index()
        {
            var landingPageViewModel = await _landingPageService.GetLandingPageInfo();
            
            return View(landingPageViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
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
