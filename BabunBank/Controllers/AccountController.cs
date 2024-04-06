using BabunBank.Configurations.Enums;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Cashier}")]
public class AccountController(AccountService accountService) : Controller
{
    public async Task<IActionResult> Details(int id)
    {
        var result = await accountService.GetAccountViewModelAsync(id);

        if (result == null)
            return RedirectToAction("Index", "Error");
        return View(result);
    }
}
