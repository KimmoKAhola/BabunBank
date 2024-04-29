using BabunBank.Factories;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Enums;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.User;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = UserRoleNames.Admin)]
public class AdminController(
    IIdentityUserService identityUserService,
    UserManager<IdentityUser> userManager,
    UserValidator userValidator
) : Controller
{
    // GET
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
    public async Task<IActionResult> Create(SignUpUserModel userModel)
    {
        var roles = DropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;
        var validationResult = await userValidator.ValidateAsync(userModel);

        if (!validationResult.IsValid)
        {
            foreach (var errorMessage in validationResult.Errors)
            {
                ModelState.AddModelError(errorMessage.PropertyName, errorMessage.ErrorMessage);
            }
            return View(userModel);
        }

        var createdUser = await IdentityUserFactory.Create(userModel, userManager);
        // var result = mapper.Map<SignUpModel>(user); //TODO Needed?
        return RedirectToAction("Details", new { id = createdUser.Id });
    }

    public async Task<IActionResult> Details(string id)
    {
        var identityUserViewModel = await identityUserService.GetSingleAsync(id);
        return View(identityUserViewModel);
    }

    public IActionResult Update()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var identityUserViewModel = await identityUserService.GetSingleAsync(id);
        return View(identityUserViewModel);
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
