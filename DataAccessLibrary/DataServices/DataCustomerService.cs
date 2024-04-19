using System.Linq.Expressions;
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
        catch (Exception)
        {
            return null;
        }
    }

    public IQueryable<Customer> GetAllAsync(string? sortColumn, string? sortOrder)
    {
        try
        {
            var sortingExpressions = new Dictionary<string, Expression<Func<Customer, object>>>
            {
                { "Id", c => c.CustomerId },
                { "Gender", c => c.Gender },
                { "GivenName", c => c.Givenname },
                { "Surname", c => c.Surname },
                { "Country", c => c.Country }
            };
            var queryResult = customerRepository.GetAllAsync();

            if (sortColumn == null || sortOrder == null)
                return queryResult;

            if (sortingExpressions.ContainsKey(sortColumn))
            {
                return sortOrder == "asc"
                    ? queryResult.OrderBy(sortingExpressions[sortColumn])
                    : queryResult.OrderByDescending(sortingExpressions[sortColumn]);
            }

            return queryResult;
        }
        catch (Exception)
        {
            return customerRepository.GetAllAsync();
        }
    }

    public async Task<bool?> CreateDepositAsync(Customer model)
    {
        try
        {
            await customerRepository.CreateAsync(model);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool?> DeleteAsync(Customer customer)
    {
        try
        {
            return await customerRepository.DeleteAsync(x => x.CustomerId == customer.CustomerId);
        }
        catch (Exception)
        {
            return false;
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
