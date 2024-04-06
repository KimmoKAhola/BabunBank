using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        Exception exception = pathFeature?.Error; // Here will be the exception details.
        return View();
    }

    public IActionResult NotFound404()
    {
        Response.StatusCode = 404;
        return View();
    }
}
