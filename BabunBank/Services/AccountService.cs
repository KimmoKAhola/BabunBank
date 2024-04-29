using AutoMapper;
using BabunBank.Configurations.Interfaces;
using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class AccountService(DataAccountService dataAccountService, IMapper mapper) : IAccountService
{
    public async Task<AccountViewModel?> GetAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        if (result == null)
            return null!;

        var accountViewModel = mapper.Map<AccountViewModel>(result);
        accountViewModel.CustomerId = result.Dispositions.First(x => x.Type == "OWNER").CustomerId; //TODO add this to automapper?

        accountViewModel.Transactions = accountViewModel
            ?.Transactions?.OrderByDescending(t => t.TransactionId)
            .Take(50) //TODO 50 is a magic string!!! Fix later
            .ToList();
        return accountViewModel;
    }

    public async Task<IEnumerable<Account>> RenameMe(int id, int pageNumber, int pageSize, string q)
    {
        var result = await dataAccountService.GetAllAccountsAndCustomersAsync(
            id,
            pageNumber,
            pageSize,
            q
        );

        return result;
    }
}
