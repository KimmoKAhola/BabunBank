﻿using AutoMapper;
using BabunBank.Models;
using BabunBank.Models.Account;
using BabunBank.Models.Customer;
using BabunBank.Models.Transaction;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService dataCustomerService, IMapper mapper)
{
    private readonly int _pageSize = 5;

    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        var result = await dataCustomerService.GetAsync(id);

        var customerViewModel = mapper.Map<CustomerViewModel>(result);
        return customerViewModel;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync(
        string sortColumn,
        string sortOrder,
        string q,
        int pageNumber
    )
    {
        var customers = dataCustomerService.GetAll(sortColumn, sortOrder);

        if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }

        var result = await customers
            .Where(x => !x.IsDeleted)
            .Skip((pageNumber - 1) * _pageSize)
            .Take(_pageSize)
            .ToListAsync();

        var customerViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(result);

        return customerViewModel;
    }

    public async Task<bool> CreateCustomerAsync(Customer customer)
    {
        return await dataCustomerService.CreateAsync(customer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await dataCustomerService.GetAsync(id);
        return await dataCustomerService.DeleteAsync(customer);
    }
}
