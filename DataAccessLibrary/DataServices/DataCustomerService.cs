using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary.DataServices;

public class DataCustomerService(CustomerRepository customerRepository)
{
    public async Task<Customer> GetCustomerAsync(int id)
    {
        try
        {
            //TODO perform include etc here. Needs more info from other tables
            var customer = await customerRepository.GetAsync(x => x.CustomerId == id);
            
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        try
        {
            var result = await customerRepository.GetAllAsync();
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}