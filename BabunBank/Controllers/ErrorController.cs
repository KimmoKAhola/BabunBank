using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NotFound404()
    {
        Response.StatusCode = 404;
        return View();
    }
}
