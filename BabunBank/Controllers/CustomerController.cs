using BabunBank.Models.Customer;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class CustomerController : Controller
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(string sortColumn, string sortOrder)
    {
        var customers = await _customerService.GetAllCustomersViewModelAsync();

        customers = customers.Take(30); // TODO
        if (sortColumn == "Gender")
        {
            if (sortOrder == "asc")
            {
                customers = customers.OrderBy(x => x.Gender);
            }
            else
            {
                customers = customers.OrderByDescending(x => x.Gender);
            }
        }
        
        else if (sortColumn == "GivenName")
        {
            if (sortOrder == "asc")
            {
                customers = customers.OrderBy(x => x.GivenName);
            }
            else
            {
                customers = customers.OrderByDescending(x => x.GivenName);
            }
        }
        
       else if (sortColumn == "Surname")
        {
            if (sortOrder == "asc")
            {
                customers = customers.OrderBy(x => x.Surname);
            }
            else
            {
                customers = customers.OrderByDescending(x => x.Surname);
            }
        }
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _customerService.GetCustomerViewModelAsync(id);


        return View(result);
    }
}