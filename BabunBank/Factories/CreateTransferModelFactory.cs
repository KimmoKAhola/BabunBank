using BabunBank.Models.FormModels.Transactions;
using BabunBank.Models.ViewModels.Account;

namespace BabunBank.Factories;

public static class CreateTransferModelFactory
{
    public static CreateDepositModel CreateDepositModel(AccountViewModel accountViewModel)
    {
        var model = new CreateDepositModel
        {
            AccountId = accountViewModel.AccountId,
            Balance = accountViewModel.Balance,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Debit",
            Operation = "Deposit"
        };

        return model;
    }

    public static CreateWithdrawalModel CreateWithdrawalModel(AccountViewModel accountViewModel)
    {
        var model = new CreateWithdrawalModel
        {
            AccountId = accountViewModel.AccountId,
            Balance = accountViewModel.Balance,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Debit",
            Operation = "Withdrawal"
        };

        return model;
    }

    public static CreateTransferModel CreateTransferModel(AccountViewModel sender)
    {
        var model = new CreateTransferModel
        {
            FromAccountId = sender.CustomerId,
            BalanceSender = sender.Balance,
            Date = DateOnly.FromDateTime(DateTime.Now),
            OperationSender = "Transfer to",
            OperationReceiver = "Deposit from"
        };

        return model;
    }
}
