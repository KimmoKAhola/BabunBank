﻿using AutoMapper;
using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models.CustomValidators;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Models.FormModels.CustomerModels;
using BabunBank.Services;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public enum CustomerDropdownMenu
{
    Twenty = 20,
    TwentyFive = 25,
    Fifty = 50,
    SeventyFive = 75,
    Hundred = 100
}

[Authorize(Roles = $"{UserRoleNames.Cashier}, {UserRoleNames.Admin}")] //TODO add these to relevant pages
public class CashierController(
    CustomerService customerService,
    MoneyLaunderingService moneyLaunderingService,
    DropDownService dropDownService,
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

        ViewBag.SortColumn = sortColumn;
        ViewBag.SortOrder = sortOrder;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = totalPageCount;

        return View(customers);
    }

    public IActionResult Clear()
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
        var result = await customerService.GetCustomerViewModelAsync(id);
        if (result != null)
            return View(result);
        TempData["ErrorMessage"] = "A CATASTROPHIC ERROR OCCURED!";
        return RedirectToAction("Index", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(int id, int pageNo)
    {
        var transactions = await customerService.GetCustomerViewModelAsync(id);

        if (transactions == null)
            return RedirectToAction("Index", "Error");

        var result = transactions
            .CustomerAccounts.FirstOrDefault(d => d.Type.ToLower() == "owner")
            ?.Transactions?.OrderByDescending(t => t.TransactionId)
            .Skip((pageNo - 1) * TransactionLimit)
            .Take(TransactionLimit)
            .ToList(); //TODO put this in a service

        if (result != null)
            return Json(result);

        return RedirectToAction("Index", "Error");
    }

    public IActionResult Create()
    {
        ViewBag.AvailableGenders = dropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = dropDownService.GetCountryOptions();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpCustomerModel model)
    {
        ViewBag.AvailableGenders = dropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = dropDownService.GetCountryOptions();

        if (!ModelState.IsValid)
            return View(model);

        var customer = CustomerFactory.Create(model, mapper);

        var result = await customerService.CreateCustomerAsync(customer);
        if (result != null)
        {
            if (!(bool)result)
                ModelState.AddModelError(string.Empty, "Failed to create customer.");
        }
        else
        {
            TempData["ErrorMessage"] = "A CATASTROPHIC ERROR OCCURED!";
            return RedirectToAction("Index", "Error");
        }

        @TempData["CreatedUser"] =
            $"Your user {customer.CustomerId} {customer.Givenname} has been created and can be seen below.";
        return RedirectToAction(nameof(Details), new { id = customer.CustomerId });
    }

    public async Task<IActionResult> Update(int id)
    {
        ViewBag.AvailableGenders = dropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = dropDownService.GetCountryOptions();

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
        ViewBag.AvailableGenders = dropDownService.GetGenderOptions();
        ViewBag.AvailableCountries = dropDownService.GetCountryOptions();

        var validation = await customerValidation.ValidateAsync(model);
        if (!validation.IsValid)
        {
            validation.AddToModelState(ModelState);
            return View(model);
        }

        if (!(bool)await customerService.UpdateCustomerAsync(model, ModelState))
            return View(model);

        return RedirectToAction(nameof(Details), new { id = id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await customerService.DeleteCustomerAsync(id);
        if (result ?? false)
        {
            return RedirectToAction("Index", "Error");
        }
        return RedirectToAction("Delete");
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

    public async Task<IActionResult> ScaleGraphs(int id, string color, int slider)
    {
        await moneyLaunderingService.InspectAccount(
            id,
            VisualizationModes.Web,
            true,
            color,
            slider
        );

        TempData["Redraw"] = true;

        return RedirectToAction("Inspect", new { id });
    }
}
