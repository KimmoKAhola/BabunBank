using BabunBank.Models.Account;
using BabunBank.Models.Cashier;
using DataAccessLibrary.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BabunBank.Factories;

public static class DepositFactory
{
    public static Transaction Create(CreateDepositModel model, AccountViewModel accountViewModel)
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
}
