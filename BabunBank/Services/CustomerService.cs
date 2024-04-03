using BabunBank.Models.Customer;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService customerService)
{
    private readonly int _pageSize = 5;
    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        var result = await customerService.GetAsync(id);

        var customer = new CustomerViewModel
        {
            Id = result.CustomerId,
            Gender = result.Gender,
            GivenName = result.Givenname,
            Surname = result.Surname,
            Streetaddress = result.Streetaddress,
            City = result.City,
            Zipcode = result.Zipcode,
            Country = result.Country
        };

        return customer;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync(string sortColumn, string sortOrder, string q, int pageNumber)
    {
        var customers = customerService.GetAll(sortColumn, sortOrder);

        if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }
        
        var result = await customers.Select(x => new CustomerViewModel
        {
            Id = x.CustomerId,
            Gender = x.Gender,
            GivenName = x.Givenname,
            Surname = x.Surname,
            Streetaddress = x.Streetaddress,
            City = x.City,
            Zipcode = x.Zipcode,
            Country = x.Country
        }).Skip((pageNumber - 1)*_pageSize)
            .Take(_pageSize)
            .ToListAsync();
        
        return result;
    }
}