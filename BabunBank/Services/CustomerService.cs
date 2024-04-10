using AutoMapper;
using BabunBank.Models.FormModels.Cashier;
using BabunBank.Models.ViewModels.Customer;
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

    public async Task<EditCustomerModel?> GetEditCustomerViewModelAsync(int id)
    {
        var result = await dataCustomerService.GetAsync(id);

        var editableCustomerModel = mapper.Map<EditCustomerModel>(result);
        return editableCustomerModel;
    }

    public async Task<(IEnumerable<CustomerViewModel>, int)> GetAllCustomersViewModelAsync(
        string sortColumn,
        string sortOrder,
        string q,
        int pageNumber,
        int pageSize
    )
    {
        var customers = dataCustomerService.GetAll(sortColumn, sortOrder);

        if (int.TryParse(q, out int value))
        {
            customers = customers.Where(x => x.CustomerId == value);
        }
        else if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }

        var result = await customers
            .Where(x => !x.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var customerViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(result);

        return (customerViewModel, customers.Count(x => !x.IsDeleted));
    }

    public async Task<bool?> CreateCustomerAsync(Customer customer)
    {
        return await dataCustomerService.CreateDepositAsync(customer);
    }

    public async Task<bool?> UpdateCustomerAsync(EditCustomerModel customerModel)
    {
        var customer = mapper.Map<Customer>(customerModel);
        return await dataCustomerService.UpdateAsync(customer);
    }

    public async Task<bool?> DeleteCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        return await dataCustomerService.DeleteAsync(customer);
    }
}
