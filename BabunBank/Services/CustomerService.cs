using AutoMapper;
using BabunBank.Models.Account;
using BabunBank.Models.Customer;
using BabunBank.Models.Transaction;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BabunBank.Services;

public class CustomerService(DataCustomerService customerService, IMapper mapper)
{
    private readonly int _pageSize = 5;
    public async Task<CustomerViewModel> GetCustomerViewModelAsync(int id)
    {
        var result = await customerService.GetAsync(id);

        var customerViewModel = new CustomerViewModel
        {
            CustomerId = result.CustomerId,
            Gender = result.Gender,
            GivenName = result.Givenname,
            Surname = result.Surname,
            Streetaddress = result.Streetaddress,
            City = result.City,
            Zipcode = result.Zipcode,
            Country = result.Country,
            CustomerAccounts = result.Dispositions
                .Select(d => new AccountViewModel
                {
                    AccountId = d.Account.AccountId,
                    Created = d.Account.Created,
                    Balance = d.Account.Balance,
                    Transactions = d.Account.Transactions.Select(t => new TransactionViewModel
                    {
                        TransactionId = t.TransactionId,
                        Date = t.Date,
                        Type = t.Type,
                        Operation = t.Operation,
                        Amount = t.Amount,
                        Balance = t.Balance,
                    }).ToList()
                })
                .ToList()
        };
        
        
        // var customerViewModel = mapper.Map<CustomerViewModel>(result);
        
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