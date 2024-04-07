using AutoMapper;
using BabunBank.Models.Customer;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService dataCustomerService, IMapper mapper)
{
    public async Task<CustomerViewModel?> GetCustomerViewModelAsync(int id)
    {
        var result = await dataCustomerService.GetAsync(id);

        var customerViewModel = mapper.Map<CustomerViewModel>(result);
        return customerViewModel;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync(
        string sortColumn,
        string sortOrder,
        string q,
        int pageNumber,
        int pageSize
    )
    {
        var customers = dataCustomerService.GetAll(sortColumn, sortOrder);

        if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }

        var result = await customers
            .Where(x => !x.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var customerViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(result);

        return customerViewModel;
    }

    public async Task<bool?> CreateCustomerAsync(Customer customer)
    {
        return await dataCustomerService.CreateAsync(customer);
    }

    public async Task<bool?> DeleteCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        return await dataCustomerService.DeleteAsync(customer);
    }
}
