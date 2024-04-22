using System.Diagnostics;
using BabunBank.Models.CustomValidators;
using BabunBank.Models.FormModels.ContactModels;
using BabunBank.Models.ViewModels.Error;
using BabunBank.Models.ViewModels.LandingPage;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers
{
    public class HomeController(
        LandingPageService landingPageService,
        ContactUsValidator contactUsValidator
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var landingPageViewModel = await landingPageService.GetLandingPageInfo();

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
            var validationResult = contactUsValidator.Validate(model);

            if (validationResult.IsValid)
                return RedirectToAction("Contact");

            validationResult.AddToModelState(ModelState);
            return View(model);
        }

        public async Task<IActionResult> Details(string country)
        {
            var result = await landingPageService.GetDetailedLandingPageInfo(country);

            return View(result);
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
