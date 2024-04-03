using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary.DataServices;

public class DataCustomerService(CustomerRepository customerRepository) : IDataService<Customer>
{
    public async Task<Customer> GetAsync(int id)
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

    public async Task<IEnumerable<Customer>> GetAllAsync()
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