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

    public async Task<IActionResult> Index()
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
        });

        return View(customers);
    }
}