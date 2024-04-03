using BabunBank.Models.Customer;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class CustomerController(CustomerService customerService) : Controller
{
    public string SortColumn { get; set; }
    public string SortOrder { get; set; }
    
    public string Country { get; set; }
    
    public async Task<IActionResult> Index(string sortColumn, string sortOrder, string countryId)
    {
        var customers = await customerService.GetAllCustomersViewModelAsync(sortColumn, sortOrder);
        
        SortColumn = sortColumn;
        SortOrder = sortOrder;

        customers = customers.Take(30); // TODO
        
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await customerService.GetCustomerViewModelAsync(id);
        
        return View(result);
    }
}