﻿using BabunBank.Factories;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.IdentityUser;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace BabunBank.Controllers;

[Authorize(Roles = UserRoleNames.Admin)]
public class AdminController(
    IIdentityUserService identityUserService,
    UserManager<IdentityUser> userManager,
    UserValidator userValidator
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var roles = DropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;

        var users = await identityUserService.GetAllAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        var roles = DropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpIdentityUserModel identityUserModel)
    {
        var roles = DropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;
        var validationResult = await userValidator.ValidateAsync(identityUserModel);

        if (!validationResult.IsValid)
        {
            foreach (var errorMessage in validationResult.Errors)
            {
                ModelState.AddModelError(errorMessage.PropertyName, errorMessage.ErrorMessage);
            }
            return View(identityUserModel);
        }

        await IdentityUserFactory.Create(identityUserModel, userManager);
        return RedirectToAction("Index");
    }

    public IActionResult Update(string id)
    {
        var roles = DropDownService.GetRoles();
        ViewBag.AvailableRoles = roles;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var identityUserViewModel = await identityUserService.GetSingleAsync(id);

        var model = new DeleteIdentityUserModel
        {
            UserId = identityUserViewModel.UserId,
            Username = identityUserViewModel.Username
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id, string username)
    {
        //TODO Fix confirmation when deleting user
        var deleteConfirmation = await identityUserService.DeleteAsync(id);
        if (!(deleteConfirmation ?? false))
            return RedirectToAction("Index");
        return RedirectToAction("Index", "Error");
    }
}
