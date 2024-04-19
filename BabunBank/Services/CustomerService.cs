using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper;
using BabunBank.Models.FormModels.Cashier;
using BabunBank.Models.ViewModels.Customer;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.DataServices.Enums;
using Microsoft.EntityFrameworkCore;
using ModelStateDictionary = System.Web.WebPages.Html.ModelStateDictionary;

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
        editableCustomerModel.CountryValue = (int)ConvertCountryToEnum(result.Country);

        return editableCustomerModel;
    }

    private static CountryOptions ConvertCountryToEnum(string country)
    {
        return Enum.TryParse<CountryOptions>(country, out var value) ? value : 0;
    }

    public async Task<(IEnumerable<CustomerViewModel>, int)> GetAllCustomersViewModelAsync(
        string? sortColumn,
        string? sortOrder,
        string? q,
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

    public async Task<bool?> UpdateCustomerAsync(
        EditCustomerModel customerModel,
        Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState
    )
    {
        if (
            await dataCustomerService.ExistsAsync(x =>
                x.Emailaddress == customerModel.EmailAddress
                && x.CustomerId != customerModel.CustomerId
            )
        )
        {
            modelState.AddModelError("EmailAddress", "That email address already exists.");
            return false;
        }

        if (
            await dataCustomerService.ExistsAsync(x =>
                x.Telephonenumber == customerModel.TelephoneNumber
                && x.CustomerId != customerModel.CustomerId
            )
        )
        {
            modelState.AddModelError("TelephoneNumber", "That phone number already exists.");
            return false;
        }

        var customer = mapper.Map<Customer>(customerModel);
        customer.Gender = Enum.GetName(typeof(GenderOptions), customerModel.GenderRole);
        customer.Country = Enum.GetName(typeof(CountryOptions), customerModel.CountryValue);
        return await dataCustomerService.UpdateAsync(customer);
    }

    public async Task<bool?> DeleteCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        return await dataCustomerService.DeleteAsync(customer);
    }

    public async Task<bool> CheckExistsByEmailAsync(string email)
    {
        return await dataCustomerService.ExistsAsync(x => x.Emailaddress == email);
    }

    public async Task<bool> CheckExistsByTelephoneNumber(string number)
    {
        return await dataCustomerService.ExistsAsync(x => x.Telephonenumber == number);
    }
}
