﻿using AutoMapper;
using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models.CustomValidators;
using BabunBank.Models.FormModels.Cashier;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Services;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public enum CustomerDropdownMenu
{
    Five = 5,
    Ten = 10,
    Fifteen = 15,
    Twenty = 20,
    TwentyFive = 25,
    Fifty = 50
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
    public async Task<IActionResult> Index(
        string sortColumn,
        string sortOrder,
        string q,
        int pageNumber,
        int pageSize
    )
    {
        if (pageNumber == 0)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 5;
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
        {
            return View(model);
        }

        var customer = CustomerFactory.Create(model, mapper);

        var result = await customerService.CreateCustomerAsync(customer);
        if (result != null)
        {
            if (!(bool)result)
            {
                ModelState.AddModelError(string.Empty, "Failed to create customer.");
            }
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
        var sus = await moneyLaunderingService.InspectAccount(id, VisualizationModes.Web);

        return View(sus);
    }
}
