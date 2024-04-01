using BabunBank.Models.Customer;
using BabunBank.Repositories;

namespace BabunBank.Services;

public class CustomerService(CustomerRepository customerRepository)
{
    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        try
        {
            //TODO perform include etc here. Needs more info from other tables
            var result = await customerRepository.GetAsync(x => x.CustomerId == id);

            var customer = new CustomerViewModel
            {
                Id = result.CustomerId,
                City = result.City,
                GivenName = result.Givenname
            };
            
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync()
    {
        try
        {
            var result = await customerRepository.GetAllAsync();

            var viewModel = result.Select(x => new CustomerViewModel
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

            return viewModel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}