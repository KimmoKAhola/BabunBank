using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace DataAccessLibrary.DataServices;

public class DataCustomerService(CustomerRepository customerRepository) : IDataService<Customer>
{
    public async Task<Customer> GetAsync(int id)
    {
        try
        {
            var customer = await customerRepository.GetAsync(x => x.CustomerId == id);

            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IQueryable<Customer> GetAll(string sortColumn, string sortOrder)
    {
        try
        {
            var result = customerRepository.GetAllAsync();

            if (sortColumn == "Gender")
            {
                if (sortOrder == "asc")
                {
                    result = result.OrderBy(x => x.Gender);
                }
                else
                {
                    result = result.OrderByDescending(x => x.Gender);
                }
            }
            else if (sortColumn == "GivenName")
            {
                if (sortOrder == "asc")
                {
                    result = result.OrderBy(x => x.Givenname);
                }
                else
                {
                    result = result.OrderByDescending(x => x.Givenname);
                }
            }
            else if (sortColumn == "Surname")
            {
                if (sortOrder == "asc")
                {
                    result = result.OrderBy(x => x.Surname);
                }
                else
                {
                    result = result.OrderByDescending(x => x.Surname);
                }
            }
            else if (sortColumn == "Country")
            {
                if (sortOrder == "asc")
                {
                    result = result.OrderBy(x => x.Country);
                }
                else
                {
                    result = result.OrderByDescending(x => x.Country);
                }
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateAsync(Customer model)
    {
        try
        {
            await customerRepository.CreateAsync(model);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Customer customer)
    {
        try
        {
            return await customerRepository.DeleteAsync(x => x.CustomerId == customer.CustomerId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Customer> UpdateAsync(Customer model)
    {
        throw new NotImplementedException();
    }
}
