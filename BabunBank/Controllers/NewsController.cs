using System.Web.Mvc;
using BabunBank.Factories.Ad;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.Ad;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BabunBank.Controllers;

public class NewsController(NewsService newsService, AdValidator adValidator) : Controller
{
    private const int StandardPageSize = 9;

    public async Task<IActionResult> Index(int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = StandardPageSize;
        }

        var (result, totalPageCount) = await newsService.GetAll(pageNumber, pageSize);

        if (result == null || totalPageCount == null)
            return RedirectToAction("Index", "Error");

        totalPageCount = (int)Math.Ceiling((double)totalPageCount / pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = totalPageCount;

        return View(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await newsService.Get(id);

        if (result == null)
            return RedirectToAction("Index", "Error");

        return View(result);
    }

    public async Task<IActionResult> Update(int id)
    {
        var blogPost = await newsService.Get(id);
        if (blogPost is null)
            return RedirectToAction("Index", "Error");
        var model = AdModelFactory.Create(blogPost);
        return View(model);
    }

    [Authorize(Roles = UserRoleNames.Admin)]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.ActionName("Update")]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, EditAdModel model)
    {
        var validation = await adValidator.ValidateAsync(model);

        if (!validation.IsValid)
        {
            validation.AddToModelState(ModelState);
            return View(model);
        }

        if (await newsService.Update(id, model))
            return RedirectToAction("Details", new { id });

        return View(model);
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var resultOfDeletion = await newsService.Delete(id);

        if (!resultOfDeletion)
            return RedirectToAction("Index", "Error");

        return View();
    }
}
