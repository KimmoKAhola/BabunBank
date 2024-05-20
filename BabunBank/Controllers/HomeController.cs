using System.Diagnostics;
using System.Web.Mvc;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.Contact;
using BabunBank.Models.ViewModels.Error;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BabunBank.Controllers;

public class HomeController(
    ILandingPageService landingPageService,
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

    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.ActionName("Contact")]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    public IActionResult Contact(
        [Microsoft.AspNetCore.Mvc.Bind("FirstName", "LastName", "Email", "Message")]
            ContactUsModel model
    )
    {
        var validationResult = contactUsValidator.Validate(model);

        if (validationResult.IsValid)
            return RedirectToAction("Contact");

        validationResult.AddToModelState(ModelState);
        return View(model);
    }

    // [ResponseCache(CacheProfileName = "Default30")] Does not work for some reason even though it follows Microsoft's documentation
    public async Task<IActionResult> Details(string country)
    {
        var result = await landingPageService.GetDetailedLandingPageInfo(country);

        return View(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
