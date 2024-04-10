using AutoMapper;
using BabunBank.Configurations;
using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models;
using BabunBank.Models.FormModels.User;
using BabunBank.Services;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabunBank.Controllers;

[Authorize(Roles = UserRoleNames.Admin)]
public class AdminController(
    IdentityUserService identityUserService,
    UserManager<IdentityUser> userManager
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
    public async Task<IActionResult> Create(SignUpUserModel userModel)
    {
        TempData["SuccessMessage"] = $"The user {userModel.UserName} has been successfully created";
        if (!ModelState.IsValid)
        {
            TempData["SuccessMessage"] = $"Incorrect values provided. Please fix";
            return View(userModel);
        }

        await IdentityUserFactory.CreateUser(userModel, userManager);
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

        var result = await identityUserService.DeleteAsync(id);
        if (!(result ?? false))
            return RedirectToAction("Index");
        TempData["SuccessMessage"] = "Something went wrong.";
        return RedirectToAction("Index", "Error");

        // return View(user);
    }
}
