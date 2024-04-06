using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabunBank.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // GET
    public IActionResult Create()
    {
        var roles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        ViewBag.AvailableRoles = roles;
        return View();
    }
}
