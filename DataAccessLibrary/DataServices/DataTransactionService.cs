using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary.DataServices;

public class DataTransactionService(TransactionRepository transactionRepository)
    : IDataService<Transaction>
{
    public Transaction Deposit(int id, decimal amount, Account account)
    {
        var transaction = new Transaction
        {
            AccountId = account.AccountId,
            Balance = account.Balance + amount,
            Date = DateOnly.FromDateTime(DateTime.Now),
        };

        return transaction;
    }

    public async Task<bool?> CreateAsync(Transaction model)
    {
        await transactionRepository.CreateAsync(model);
        return true;
    }

    public async Task<Transaction?> Transfer()
    {
        // var fromAccount = await dataAccountService.GetAsync(fromAccId);
        // var toAccount = await dataAccountService.GetAsync(toAccId);
        //
        // if (fromAccount == null || toAccount == null)
        //     return false;
        // if (amount > fromAccount.Balance)
        //     return false;
        //
        // fromAccount.Balance -= amount;
        // toAccount.Balance += amount;
        //
        // await dataAccountService.UpdateAsync(fromAccount);
        // await dataAccountService.UpdateAsync(toAccount);
        // return true;

        return null!;
    }

    public async Task<Transaction?> Withdraw()
    {
        // var account = await dataAccountService.GetAsync(id);
        //
        // if (account == null)
        //     return false;
        // if (amount > account.Balance)
        // {
        //     return false;
        // }
        //
        // account.Balance -= amount;
        // await dataAccountService.UpdateAsync(account);
        // return true;

        return null!;
    }

    public async Task<Transaction?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Transaction> GetAll(string sortOrder, string sortColumn)
    {
        throw new NotImplementedException();
    }

    public async Task<bool?> DeleteAsync(Transaction model)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction?> UpdateAsync(Transaction model)
    {
        throw new NotImplementedException();
    }
}
