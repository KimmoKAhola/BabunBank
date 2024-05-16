using BabunBank.Factories;
using BabunBank.Factories.Users;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.IdentityUser;
using DataAccessLibrary.Data;
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
        ViewBag.AvailableRoles = DropDownService.GetRoles();
        var users = await identityUserService.GetAllAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        ViewBag.AvailableRoles = DropDownService.GetRoles();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpIdentityUserModel identityUserModel)
    {
        ViewBag.AvailableRoles = DropDownService.GetRoles();
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

    public async Task<IActionResult> Update(string id)
    {
        ViewBag.AvailableRoles = DropDownService.GetRoles();
        var identityUserViewModel = await identityUserService.GetSingleAsync(id);

        var updateModel = new UpdateIdentityUserModel
        {
            UserId = identityUserViewModel.UserId,
            OldEmail = identityUserViewModel.Email,
            UserRole = (UserRole)Enum.Parse(typeof(UserRole), identityUserViewModel.RoleName)
        };

        return View(updateModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateIdentityUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var resultOfUpdate = await identityUserService.UpdateAsync(model);
        if (resultOfUpdate.Succeeded)
        {
            return RedirectToAction("Index");
        }

        ViewBag.AvailableRoles = DropDownService.GetRoles();
        foreach (var error in resultOfUpdate.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        var newModel = new UpdateIdentityUserModel
        {
            OldEmail = identityUserService.GetSingleAsync(model.UserId).Result.Email
        };
        return View(newModel);
    }

    public IActionResult UpdatePassword(string id, string username)
    {
        var model = new UpdateIdentityUserPasswordModel { UserId = id, Username = username };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword(UpdateIdentityUserPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await identityUserService.UpdatePasswordAsync(model.UserId, model);

        if (result.Succeeded)
            return RedirectToAction("Index");

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(
                error.Code == "PasswordMismatch" ? "OldPassword" : "NewPassword",
                error.Description
            );
        }
        return View(model);
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
    public async Task<IActionResult> Delete(DeleteIdentityUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
            //TODO smth here
        }
        var deleteConfirmation = await identityUserService.DeleteAsync(model.UserId);
        if (!deleteConfirmation)
        {
            return RedirectToAction("Index", "Error");
        }
        return RedirectToAction("Index");
    }
}
