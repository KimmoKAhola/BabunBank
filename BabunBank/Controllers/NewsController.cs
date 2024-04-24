using System.Web.Mvc;
using BabunBank.Configurations.Enums;
using BabunBank.Models.CustomValidators;
using BabunBank.Models.FormModels.AdModels;
using BabunBank.Models.FormModels.User;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BabunBank.Controllers;

public class NewsController(NewsService newsService, AdValidator adValidator) : Controller
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
        var result = await newsService.Get(id);

        var model = new EditAdModel
        {
            Title = result!.Title,
            Author = result.Author,
            Content = result.Content,
            Description = result.Description,
            IsDeleted = false
        };

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
        var result = await newsService.Delete(id);

        if (result == false)
            return RedirectToAction("Index", "Error");

        return View();
    }

    // public async Task<IActionResult> Authenticate()
    // {
    //     return View();
    // }

    public async Task<IActionResult> Authenticate(User user)
    {
        var user2 = new User { UserName = "richard.erdos.chalk@gmail.se", Password = "Hejsan123#" };

        var result = await newsService.Test(user2);
        ViewBag.Test = result;
        return RedirectToAction("Index", "News");
    }
}
