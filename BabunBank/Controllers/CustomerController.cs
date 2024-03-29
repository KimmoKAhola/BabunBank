using BabunBank.Models.Customer;
using BabunBank.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class CustomerController : Controller
{
    private readonly CustomerRepository _customerRepository;

    public CustomerController(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IActionResult> Index(string sortColumn, string sortOrder)
    {
        var temp = await _customerRepository.GetAllAsync();

        var customers = temp.Select(x => new CustomerViewModel
        {
            Gender = x.Gender,
            GivenName = x.Givenname,
            Surname = x.Surname,
            Streetaddress = x.Streetaddress,
            City = x.City,
            Zipcode = x.Zipcode,
            Country = x.Country
        }).Take(20);

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
}