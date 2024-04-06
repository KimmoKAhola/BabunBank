using BabunBank.Configurations;
using BabunBank.Services;
using DataAccessLibrary.Data;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabunBank.Controllers;

[Authorize(Roles = RoleNames.Admin)]
public class AdminController(IdentityUserService identityUserService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var roles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        ViewBag.AvailableRoles = roles;

        var users = await identityUserService.GetAllAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        var roles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        ViewBag.AvailableRoles = roles;
        return View();
    }

    public async Task<IActionResult> Read(string id)
    {
        var user = await identityUserService.GetSingleAsync(id);
        return View(user);
    }

    public IActionResult Update()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await identityUserService.GetSingleAsync(id);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id, string username)
    {
        TempData["SuccessMessage"] =
            $"User with username \"{@username}\" has been deleted successfully.";

        if (!await identityUserService.DeleteAsync(id))
        {
            TempData["SuccessMessage"] = "Something went wrong.";
        }

        return RedirectToAction("Index");
        // return View(user);
    }
}
