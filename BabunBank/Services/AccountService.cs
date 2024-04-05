using BabunBank.Models.Account;
using BabunBank.Models.Transaction;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class AccountService(DataAccountService dataAccountService)
{
    public async Task<AccountViewModel> GetAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        var account = new AccountViewModel
        {
            AccountId = result.AccountId,
            Created = result.Created,
            Balance = result.Balance,
            Transactions = result.Transactions.Select(t => new TransactionViewModel
            {
                TransactionId = t.TransactionId,
                Date = t.Date,
                Type = t.Type,
                Operation = t.Operation,
                Amount = t.Amount,
                Balance = t.Balance
            }).OrderByDescending(t => t.Date).ToList()
        };
        
        return account;
    }
}