using AutoMapper;
using BabunBank.Models.FormModels.Cashier;
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

    public async Task<bool?> CreateTransferAsync(CreateTransferModel transfer)
    {
        var deposit = new Transaction
        {
            AccountId = transfer.ToAccountId,
            Date = transfer.Date,
            Type = transfer.Type,
            Operation = $"{transfer.OperationSender}" + $"{transfer.ToAccountId}",
            Amount = transfer.Amount,
            Balance = transfer.Balance,
            Symbol = transfer.Symbol,
            Bank = transfer.Bank,
            Account = transfer.Account
        };

        var withdrawal = new Transaction
        {
            AccountId = transfer.FromAccountId,
            Date = transfer.Date,
            Type = transfer.Type,
            Operation = $"{transfer.OperationReceiver}" + $"{transfer.FromAccountId}",
            Amount = (transfer.Amount * -1),
            Balance = transfer.Balance,
            Symbol = transfer.Symbol,
            Bank = transfer.Bank,
            Account = transfer.Account
        };
        return await dataTransactionService.CreateTransferAsync(deposit, withdrawal);
    }
}
