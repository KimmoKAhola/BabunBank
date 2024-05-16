using AutoMapper;
using BabunBank.Factories;
using BabunBank.Factories.Users;
using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.Customer;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Cashier}, {UserRoleNames.Admin}")]
public class CashierController(
    ICustomerService customerService,
    IMoneyLaunderingService moneyLaunderingService,
    IMapper mapper,
    EditCustomerValidation customerValidation
) : Controller
{
    private const int TransactionLimit = 20;

    public async Task<IActionResult> Index(
        string sortColumn,
        string sortOrder,
        string q,
        int pageNumber = 0,
        int pageSize = 50
    )
    {
        if (pageNumber == 0)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 50;
        }

        var (customers, totalPageCount) = await customerService.GetAllCustomersViewModelAsync(
            sortColumn,
            sortOrder,
            q,
            pageNumber,
            pageSize
        );

        totalPageCount = (int)Math.Ceiling((double)totalPageCount / pageSize);

        if (!customers.Any() && totalPageCount == 0)
            return RedirectToAction("Index", "Error");

        ViewBag.SortColumn = sortColumn;
        ViewBag.SortOrder = sortOrder;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = totalPageCount;

        return View(customers);
    }

    public IActionResult ClearFilters()
    {
        ViewBag.SortColumn = "";
        ViewBag.SortOrder = "";
        ViewBag.CurrentPage = 1;
        ViewBag.Q = "";
        ViewBag.PageSize = 0;
        ViewBag.TotalPageCount = 0;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var message = TempData["Message"] as string;
        var notification = TempData["Notification"] as string;
        if (!string.IsNullOrEmpty(message))
        {
            ViewBag.Message = message;
        }
        if (!string.IsNullOrEmpty(notification))
        {
            ViewBag.Notification = notification;
        }
        var result = await customerService.GetCustomerViewModelAsync(id);
        if (result != null)
            return View(result);

        return RedirectToAction("Index", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(int id, int pageNo)
    {
        var transactions = await customerService.GetCustomerViewModelAsync(id);

        if (transactions == null)
            return RedirectToAction("Index", "Error");

        var result = transactions
            .CustomerAccounts.SelectMany(a => a.Transactions)
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.TransactionId)
            .Skip((pageNo - 1) * TransactionLimit)
            .Take(TransactionLimit)
            .ToList(); //TODO put this in a service

        if (result != null)
            return Json(result);

        return RedirectToAction("Index", "Error");
    }

    public IActionResult Create()
    {
        ViewBag.AvailableGenders = DropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = DropDownService.GetCountryOptions();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpCustomerModel model)
    {
        ViewBag.AvailableGenders = DropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = DropDownService.GetCountryOptions();

        if (!ModelState.IsValid)
            return View(model);

        var customer = CustomerFactory.Create(model, mapper);

        var resultOfCreatedCustomer = await customerService.CreateCustomerAsync(customer);
        if (resultOfCreatedCustomer != null)
        {
            if (!(bool)resultOfCreatedCustomer)
                ModelState.AddModelError(string.Empty, "Failed to create customer.");
        }
        else
        {
            TempData["ErrorMessage"] = "A CATASTROPHIC ERROR OCCURED!";
            return RedirectToAction("Index", "Error");
        }

        TempData["Message"] = "Customer successfully created!";
        return RedirectToAction(nameof(Details), new { id = customer.CustomerId });
    }

    public async Task<IActionResult> Update(int id)
    {
        ViewBag.AvailableGenders = DropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = DropDownService.GetCountryOptions();

        var model = await customerService.GetEditCustomerViewModelAsync(id);
        if (model == null)
            return RedirectToAction("Index", "Error");

        return View(model);
    }

    [HttpPost, ActionName("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        int id,
        [Bind(
            "CustomerId, GivenName, GenderRole, SurName, StreetAddress, City, Zipcode, CountryValue, CountryCode, TelephoneCountryCode, TelephoneNumber, EmailAddress, NationalId, Birthday"
        )]
            EditCustomerModel model
    )
    {
        ViewBag.AvailableGenders = DropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = DropDownService.GetCountryOptions();

        var validation = await customerValidation.ValidateAsync(model);
        if (!validation.IsValid)
        {
            validation.AddToModelState(ModelState);
            return View(model);
        }

        if (!(bool)await customerService.UpdateCustomerAsync(model, ModelState))
            return View(model);

        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var customer = await customerService.GetCustomerViewModelAsync(id);
        if (customer == null)
        {
            return RedirectToAction("Index", "Error");
        }
        var model = new DeleteCustomerModel
        {
            FirstName = customer.GivenName!,
            Surname = customer.Surname!,
            Id = customer.CustomerId
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCustomerModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await customerService.DeleteCustomerAsync(model.Id);
        if (result is false or null)
        {
            return RedirectToAction("Index", "Error");
        }

        return RedirectToAction("Index", "Cashier");
    }

    public async Task<IActionResult> Inspect(int id)
    {
        bool drawGraphs = TempData["Redraw"] == null;
        var resultOfInspection = await moneyLaunderingService.InspectAccount(
            id,
            VisualizationModes.Web,
            drawGraphs
        );

        if (resultOfInspection.TransactionsOverLimit.Count == 0)
            TempData["Message"] =
                "There are too few transactions made on this account. No data to show.";

        return View(resultOfInspection);
    }

    public async Task<IActionResult> CreateAccount(int id)
    {
        var createNewAccount = await customerService.AddAccountToCustomerAsync(id);
        if (createNewAccount is false)
        {
            TempData["Notification"] = "You can not create more than 5 accounts";
        }

        if (createNewAccount is null)
        {
            return RedirectToAction("Index", "Error");
        }
        return RedirectToAction("Details", "Cashier", new { id });
    }

    public async Task<IActionResult> ScaleGraphs(
        int id,
        string color,
        string backgroundColor,
        int slider
    )
    {
        await moneyLaunderingService.InspectAccount(
            id,
            VisualizationModes.Web,
            true,
            color,
            backgroundColor,
            slider
        );

        TempData["Redraw"] = true;

        return RedirectToAction("Inspect", new { id });
    }
}
