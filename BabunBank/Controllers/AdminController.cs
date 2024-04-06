using AutoMapper;
using BabunBank.Configurations;
using BabunBank.Factories;
using BabunBank.Models;
using BabunBank.Services;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabunBank.Controllers;

[Authorize(Roles = RoleNames.Admin)]
public class AdminController(
    IdentityUserService identityUserService,
    UserManager<IdentityUser> userManager,
    IMapper mapper
) : Controller
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

    [HttpPost]
    public async Task<IActionResult> Create(SignUpModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await IdentityUserFactory.CreateUser(model, userManager);
        // var result = mapper.Map<SignUpModel>(user); //TODO Needed?
        return RedirectToAction("Index");
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
