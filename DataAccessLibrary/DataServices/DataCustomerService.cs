using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary.DataServices;

public class DataCustomerService(CustomerRepository customerRepository) : IDataService<Customer>
{
    public async Task<Customer?> GetAsync(int id)
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

            switch (sortColumn)
            {
                case "Id" when sortOrder == "asc":
                    result = result.OrderBy(x => x.CustomerId);
                    break;
                case "Id":
                    result = result.OrderByDescending(x => x.CustomerId);
                    break;
                case "Gender" when sortOrder == "asc":
                    result = result.OrderBy(x => x.Gender);
                    break;
                case "Gender":
                    result = result.OrderByDescending(x => x.Gender);
                    break;
                case "GivenName" when sortOrder == "asc":
                    result = result.OrderBy(x => x.Givenname);
                    break;
                case "GivenName":
                    result = result.OrderByDescending(x => x.Givenname);
                    break;
                case "Surname" when sortOrder == "asc":
                    result = result.OrderBy(x => x.Surname);
                    break;
                case "Surname":
                    result = result.OrderByDescending(x => x.Surname);
                    break;
                case "Country" when sortOrder == "asc":
                    result = result.OrderBy(x => x.Country);
                    break;
                case "Country":
                    result = result.OrderByDescending(x => x.Country);
                    break;
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool?> CreateDepositAsync(Customer model)
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

    public async Task<bool?> DeleteAsync(Customer customer)
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

    public async Task<bool?> UpdateAsync(Customer model)
    {
        return await customerRepository.UpdateAsync(x => x.CustomerId == model.CustomerId, model)
            != null;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Customer, bool>> predicate)
    {
        return await customerRepository.ExistsAsync(predicate);
    }
}
