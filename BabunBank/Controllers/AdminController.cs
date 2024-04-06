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

    public IActionResult Read()
    {
        return View();
    }

    public IActionResult Update()
    {
        return View();
    }

    public IActionResult Delete()
    {
        return View();
    }
}
