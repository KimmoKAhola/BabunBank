using BabunBank.Configurations;
using BabunBank.Models.Account;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.Cashier}")]
public class AccountController(AccountService accountService) : Controller
{
    public async Task<IActionResult> Details(int id)
    {
        var result = await accountService.GetAccountViewModelAsync(id);

        return View(result);
    }
}
