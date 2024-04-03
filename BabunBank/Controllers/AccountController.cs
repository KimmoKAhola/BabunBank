using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}