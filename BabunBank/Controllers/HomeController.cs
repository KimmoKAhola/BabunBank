using System.Diagnostics;
using BabunBank.Models;
using BabunBank.Models.FormModels.Home;
using BabunBank.Models.ViewModels.Error;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost, ActionName("Contact")]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(
            [Bind("FirstName", "LastName", "Email", "Message")] ContactUsModel model
        )
        {
            if (!ModelState.IsValid)
                return View(model);

            //TODO Add something nice here!
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
