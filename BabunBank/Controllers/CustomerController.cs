using BabunBank.Models.Customer;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class CustomerController(CustomerService customerService) : Controller
{
    public async Task<IActionResult> Index(string sortColumn, string sortOrder, string q, int pageNumber)
    {
        if (pageNumber == 0)
        {
            pageNumber = 1;
        }
        
        var customers = await customerService.GetAllCustomersViewModelAsync(sortColumn, sortOrder, q, pageNumber);
        
        ViewBag.SortColumn = sortColumn;
        ViewBag.SortOrder = sortOrder;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await customerService.GetCustomerViewModelAsync(id);
        //result > list of CustomerAccounts. Each account has a list of transactions
        return View(result);
    }
}