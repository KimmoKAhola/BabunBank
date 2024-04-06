using AutoMapper;
using BabunBank.Models.Account;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class AccountService(DataAccountService dataAccountService, IMapper mapper)
{
    public async Task<AccountViewModel> GetAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        if (result == null)
            return null!;

        var accountViewModel = mapper.Map<AccountViewModel>(result);
        accountViewModel.Transactions = accountViewModel
            .Transactions.OrderByDescending(t => t.Date)
            .ToList();
        return accountViewModel;
    }
}
