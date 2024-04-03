using BabunBank.Models.Customer;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService customerService)
{
    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        var result = await customerService.GetCustomerAsync(id);

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

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync()
    {
        var result = await customerService.GetAllCustomersAsync();

        var customers = result.Select(x => new CustomerViewModel
        {
            Id = x.CustomerId,
            Gender = x.Gender,
            GivenName = x.Givenname,
            Surname = x.Surname,
            Streetaddress = x.Streetaddress,
            City = x.City,
            Zipcode = x.Zipcode,
            Country = x.Country
        });

        return customers;
    }
}