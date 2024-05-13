using AutoMapper;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.Transactions;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class TransactionService(DataTransactionService dataTransactionService) : ITransactionService
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
        //TODO implement automapper here
        var deposit = new Transaction
        {
            AccountId = transfer.ToAccountId,
            Date = transfer.Date,
            Type = transfer.Type,
            Operation = $"{transfer.OperationReceiver}" + $" account {transfer.FromAccountId}",
            Amount = transfer.Amount,
            Balance = transfer.BalanceReceiver,
            Symbol = transfer.Symbol,
            Bank = transfer.Bank,
            Account = transfer.Account
        };

        var withdrawal = new Transaction
        {
            AccountId = transfer.FromAccountId,
            Date = transfer.Date,
            Type = transfer.Type,
            Operation = $"{transfer.OperationSender}" + $" account {transfer.ToAccountId}",
            Amount = (transfer.Amount * -1),
            Balance = transfer.BalanceSender,
            Symbol = transfer.Symbol,
            Bank = transfer.Bank,
            Account = transfer.Account
        };
        return await dataTransactionService.CreateTransferAsync(deposit, withdrawal);
    }
}
