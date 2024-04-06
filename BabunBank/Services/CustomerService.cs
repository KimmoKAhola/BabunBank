using AutoMapper;
using BabunBank.Models.Account;
using BabunBank.Models.Customer;
using BabunBank.Models.Transaction;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService customerService, IMapperBase mapper)
{
    private readonly int _pageSize = 5;
    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        var result = await customerService.GetAsync(id);

        var customerViewModel = mapper.Map<CustomerViewModel>(result);
        return customerViewModel;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersViewModelAsync(string sortColumn, string sortOrder, string q, int pageNumber)
    {
        var customers = customerService.GetAll(sortColumn, sortOrder);

        if (!string.IsNullOrEmpty(q))
        {
            customers = customers.Where(x => x.Givenname.Contains(q) || x.Surname.Contains(q));
        }
        
        var result = await customers.Skip((pageNumber - 1)*_pageSize)
            .Take(_pageSize)
            .ToListAsync();
        
        var customerViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(result);
        
        return customerViewModel;
    }
}