using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class AccountController(AccountService accountService) : Controller
{
    public async Task<IActionResult> Index(int id)
    {
        var result = await accountService.GetAccountViewModelAsync(id);
        
        return View();
    }
}