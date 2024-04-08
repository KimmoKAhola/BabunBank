using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class TransactionService(DataTransactionService dataTransactionService)
{
    public async Task<bool?> CreateDepositAsync(Transaction transaction)
    {
        return await dataTransactionService.CreateDepositAsync(transaction);
    }

    public async Task<bool?> CreateWithdrawalAsync(Transaction transaction)
    {
        return await dataTransactionService.CreateWithdrawalAsync(transaction);
    }

    public async Task<bool?> CreateTransferAsync(Transaction deposit, Transaction withdrawal)
    {
        return await dataTransactionService.CreateTransferAsync(deposit, withdrawal);
    }
}
