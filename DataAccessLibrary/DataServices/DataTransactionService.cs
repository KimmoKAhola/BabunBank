using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataTransactionService(
    TransactionRepository transactionRepository,
    AccountRepository accountRepository
) : IDataService<Transaction>
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
        //Get the account associated with the model
        var account = await accountRepository.GetAsync(x => x.AccountId == model.AccountId);
        if (account == null)
            return null!;

        //Create a new transaction
        var newTransaction = await transactionRepository.CreateAsync(model);

        //Sum of all transaction amounts
        var newAccountBalance = await transactionRepository
            .GetAllAsync()
            .Where(x => x.AccountId == newTransaction.AccountId)
            .SumAsync(x => x.Amount);

        //Update the balance according to transactions
        account.Balance = newAccountBalance;

        //Update the account. Check for null here? TODO
        await accountRepository.UpdateAsync(x => x.AccountId == account.AccountId, account);
        return true;
    }

    public async Task<Transaction?> Transfer()
    {
        //TODO This can use deposit and withdraw since they have the logic already
        return null!;
    }

    public async Task<Transaction?> Withdraw()
    {
        //TODO check the deposit

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

    public IQueryable<Transaction> GetAll()
    {
        return transactionRepository.GetAllAsync();
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
