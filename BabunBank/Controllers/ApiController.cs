using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class ApiController(ApiService apiService) : Controller
{
    public async Task<IActionResult> Index(int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 9;
        }

        var (result, totalPageCount) = await apiService.GetAll(pageNumber, pageSize);

        totalPageCount = (int)Math.Ceiling((double)totalPageCount / pageSize);

        if (result == null)
            return RedirectToAction("Index", "Error");

        ViewBag.CurrentPage = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = totalPageCount;

        return View(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await apiService.Get(id);

        if (result == null)
            return RedirectToAction("Index", "Error");

        return View(result);
    }
}
