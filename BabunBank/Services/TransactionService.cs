using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class TransactionService(DataTransactionService dataTransactionService)
{
    public async Task<Transaction> GetTransaction()
    {
        return await dataTransactionService.GetAsync(960);
    }

    public async Task<bool?> CreateTransactionAsync(Transaction transaction)
    {
        return await dataTransactionService.CreateAsync(transaction);
    }
}
