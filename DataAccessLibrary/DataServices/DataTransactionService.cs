using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataTransactionService(
    TransactionRepository transactionRepository,
    AccountRepository accountRepository
)
{
    public async Task<bool?> CreateDepositAsync(Transaction deposit)
    {
        var account = await accountRepository.GetAsync(x => x.AccountId == deposit.AccountId);
        if (account == null)
            return null!;

        var newTransaction = await transactionRepository.CreateAsync(deposit);

        if (newTransaction == null)
            return false;

        var newAccountBalance = await transactionRepository
            .GetAllAsync()
            .Where(x => x.AccountId == newTransaction.AccountId)
            .SumAsync(x => x.Amount);

        account.Balance = newAccountBalance;

        await accountRepository.UpdateAsync(x => x.AccountId == account.AccountId, account);
        return true;
    }

    public async Task<bool?> CreateWithdrawalAsync(Transaction withdrawal)
    {
        var account = await accountRepository.GetAsync(x => x.AccountId == withdrawal.AccountId);
        if (account == null)
            return null!;

        var newTransaction = await transactionRepository.CreateAsync(withdrawal);

        if (newTransaction == null)
            return false;

        var newAccountBalance = await transactionRepository
            .GetAllAsync()
            .Where(x => x.AccountId == newTransaction.AccountId)
            .SumAsync(x => x.Amount);

        account.Balance = newAccountBalance;

        await accountRepository.UpdateAsync(x => x.AccountId == account.AccountId, account);
        return true;
    }

    public async Task<bool> CreateTransferAsync(Transaction deposit, Transaction withdrawal)
    {
        try
        {
            var result = await transactionRepository.CreateTransferAsync(deposit, withdrawal);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
