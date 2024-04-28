using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering.Models;
using Microsoft.EntityFrameworkCore;

namespace DetectMoneyLaundering.Services;

//TODO Rename me to something better
public class MoneyLaunderingService(DataAccountService dataAccountService)
{
    public async Task<Account?> GetAccount(int id)
    {
        return await dataAccountService.GetAsync(id);
    }

    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        return await dataAccountService.GetAllAsync().ToListAsync();
    }

    public async Task<InspectAccountModel> InspectAccount(int id, VisualizationModes mode)
    {
        var result = new InspectAccountModel();
        var account = await GetAccount(id);

        if (account == null)
            return result;

        foreach (var transaction in account.Transactions)
        {
            // transaction.Amount = Math.Abs(transaction.Amount);
            if (Math.Abs(transaction.Amount) > 15000)
            {
                result.TransactionsOverLimit.Add(transaction);
            }
            else
            {
                result.NormalTransactions.Add(transaction);
            }
        }

        result.TotalNumberOfTransactions = account.Transactions.Count;

        result.CustomerName = account
            .Dispositions.First(x => x.Type.ToLower() == "owner")
            .Customer.Givenname;
        result.CustomerId = account.Dispositions.First(x => x.Type.ToLower() == "owner").CustomerId;
        if (result.NormalTransactions.Count <= 0 || result.TransactionsOverLimit.Count <= 0)
            return result;
        var scaling = new PlotScalingModel
        {
            HeightScaleFactor = 1.0,
            WidthScaleFactor = 1.0,
            FontScaleFactor = 1.0
        };
        DataVisualizationService.CreateIndividualPlot(result, mode, scaling);
        return result;
    }

    public async Task<List<InspectAccountModel>> InspectAllAccounts()
    {
        var accountsToInspect = new List<InspectAccountModel>();
        var allAccounts = await dataAccountService.GetAllAsync().ToListAsync();

        foreach (var account in allAccounts)
        {
            var temp = new InspectAccountModel();
            accountsToInspect.Add(temp);
            foreach (var transaction in account.Transactions)
            {
                // transaction.Amount = Math.Abs(transaction.Amount);
                if (Math.Abs(transaction.Amount) > 15000M)
                {
                    temp.TransactionsOverLimit.Add(transaction);
                }
                else
                {
                    temp.NormalTransactions.Add(transaction);
                }
            }

            temp.TotalNumberOfTransactions =
                temp.NormalTransactions.Count + temp.TransactionsOverLimit.Count;

            var ownerDisposition = account.Dispositions.First(x => x.Type.ToLower() == "owner");

            temp.CustomerName =
                ownerDisposition.Customer.Givenname + " " + ownerDisposition.Customer.Surname;
            temp.CustomerId = ownerDisposition.CustomerId;
        }

        return accountsToInspect;
    }
}
