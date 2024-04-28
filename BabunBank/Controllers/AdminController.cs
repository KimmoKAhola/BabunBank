using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models.CustomValidators;
using BabunBank.Models.FormModels.User;
using BabunBank.Services;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = UserRoleNames.Admin)]
public class AdminController(
    IdentityUserService identityUserService,
    UserManager<IdentityUser> userManager,
    DropDownService dropDownService,
    UserValidator userValidator
) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var roles = dropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;

        var users = await identityUserService.GetAllAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        var roles = dropDownService.GetRoles();

        ViewBag.AvailableRoles = roles;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpUserModel userModel)
    {
        var roles = dropDownService.GetRoles();

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

        var createdUser = await IdentityUserFactory.CreateUser(userModel, userManager);
        // var result = mapper.Map<SignUpModel>(user); //TODO Needed?
        return RedirectToAction("Details", new { id = createdUser.Id });
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
        //TODO Fix confirmation when deleting user
        var deleteConfirmation = await identityUserService.DeleteAsync(id);
        if (!(deleteConfirmation ?? false))
            return RedirectToAction("Index");
        return RedirectToAction("Index", "Error");
    }
}
