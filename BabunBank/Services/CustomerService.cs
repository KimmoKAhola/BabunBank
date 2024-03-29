using BabunBank.Models.Customer;
using BabunBank.Repositories;

namespace BabunBank.Services;

public class CustomerService(CustomerRepository customerRepository)
{
    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync()
    {
        try
        {
            var result = await customerRepository.GetAllAsync();

            var viewModel = result.Select(x => new CustomerViewModel
            {
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