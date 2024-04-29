using BabunBank.Models.FormModels.CustomerModels;
using BabunBank.Models.ViewModels.Customer;
using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<CustomerViewModel?> GetCustomerViewModelAsync(int id);
    Task<EditCustomerModel?> GetEditCustomerViewModelAsync(int id);
    Task<(IEnumerable<CustomerViewModel>, int)> GetAllCustomersViewModelAsync(
        string? sortColumn,
        string? sortOrder,
        string? q,
        int pageNumber,
        int pageSize
    );
    Task<bool?> CreateCustomerAsync(Customer customer);
    Task<bool?> UpdateCustomerAsync(
        EditCustomerModel customerModel,
        Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState
    );
    Task<bool?> DeleteCustomerAsync(int id);
}
