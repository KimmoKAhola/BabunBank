using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
