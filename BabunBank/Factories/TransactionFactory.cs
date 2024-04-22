using AutoMapper;
using BabunBank.Models.FormModels.TransferModels;
using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;

namespace BabunBank.Factories;

public static class TransactionFactory
{
    public static Transaction CreateDeposit(CreateDepositModel model)
    {
        try
        {
            var transaction = new Transaction
            {
                AccountId = model.AccountId,
                Date = model.Date,
                Type = model.Type,
                Operation = model.Operation,
                Amount = model.Amount,
                Balance = model.Balance,
                Symbol = model.Symbol,
                Bank = model.Bank,
                Account = model.Account
            };

            return transaction;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static Transaction CreateWithdrawal(CreateWithdrawalModel model)
    {
        try
        {
            var transaction = new Transaction
            {
                AccountId = model.AccountId,
                Date = model.Date,
                Type = model.Type,
                Operation = model.Operation,
                Amount = (model.Amount * -1),
                Balance = model.Balance,
                Symbol = model.Symbol,
                Bank = model.Bank,
                Account = model.Account
            };

            return transaction;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static CreateTransferModel CreateTransfer(
        CreateTransferModel model,
        AccountViewModel receiver
    )
    {
        try
        {
            var transfer = new CreateTransferModel
            {
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
                Amount = model.Amount,
                Type = model.Type,
                BalanceSender = model.BalanceSender,
                BalanceReceiver = receiver.Balance,
                Symbol = model.Symbol,
                Bank = model.Symbol,
                Account = model.Account
            };

            return transfer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
