﻿using AutoMapper;
using BabunBank.Factories.Account;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Models.ViewModels.Customer;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.DataServices.Enums;
using Microsoft.EntityFrameworkCore;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService dataCustomerService, IMapper mapper)
    : ICustomerService
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
        editableCustomerModel.CountryValue = (int)ConvertCountryToEnum(result!.Country);
        editableCustomerModel.GenderRole = (int)ConvertGenderToEnum(result.Gender);

        return editableCustomerModel;
    }

    public async Task<(IEnumerable<CustomerViewModel>, int)> GetAllCustomersViewModelAsync(
        string? sortColumn,
        string? sortOrder,
        string? q,
        int pageNumber,
        int pageSize
    )
    {
        var customers = dataCustomerService.GetAllAsync(sortColumn, sortOrder);

        if (int.TryParse(q, out int value))
        {
            customers = customers.Where(x => x.CustomerId == value);
        }
        else if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }

        try
        {
            var result = await customers
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var customerViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(result);

            return (customerViewModel, customers.Count(x => !x.IsDeleted));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (new List<CustomerViewModel>(), 0);
        }
    }

    public async Task<bool?> CreateCustomerAsync(Customer customer)
    {
        customer.Dispositions = new List<Disposition>();
        var account = AccountFactory.Create();
        customer.Dispositions.Add(
            new Disposition
            {
                Account = account,
                Customer = customer,
                Type = DispositionTypeNames.Owner
            }
        );
        return await dataCustomerService.CreateAsync(customer);
    }

    public async Task<bool?> AddAccountToCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        if (customer == null)
            return null;
        if (customer.Dispositions.Count >= MaximumNumberOfAccounts)
            return false;
        var newAccount = AccountFactory.Create();
        customer.Dispositions.Add(
            new Disposition
            {
                Account = newAccount,
                Customer = customer,
                Type = DispositionTypeNames.Owner
            }
        );

        return await dataCustomerService.UpdateAsync(customer);
    }

    private const int MaximumNumberOfAccounts = 5;

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
        customer.Gender = Enum.GetName(typeof(GenderOptions), customerModel.GenderRole)!;
        customer.Country = Enum.GetName(typeof(CountryOptions), customerModel.CountryValue)!;
        return await dataCustomerService.UpdateAsync(customer);
    }

    public async Task<bool?> DeleteCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        if (customer is null)
            return null;
        return await dataCustomerService.DeleteAsync(customer);
    }

    private static CountryOptions ConvertCountryToEnum(string country)
    {
        return Enum.TryParse<CountryOptions>(country, out var value) ? value : 0;
    }

    private static GenderOptions ConvertGenderToEnum(string gender)
    {
        return Enum.TryParse<GenderOptions>(gender, out var value) ? value : 0;
    }
}
