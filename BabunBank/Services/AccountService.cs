using BabunBank.Models.Account;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class AccountService(DataAccountService dataAccountService)
{
    public async Task<AccountViewModel> GetAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        var account = new AccountViewModel
        {
            Id = result.AccountId,
            Created = result.Created,
            Balance = result.Balance
        };
        
        return account;
    }
}