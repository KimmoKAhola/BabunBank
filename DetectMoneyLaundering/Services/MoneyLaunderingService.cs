using System.Drawing;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering.Interfaces;
using DetectMoneyLaundering.Models;
using Microsoft.EntityFrameworkCore;

namespace DetectMoneyLaundering.Services;

public class MoneyLaunderingService(DataAccountService dataAccountService) : IMoneyLaunderingService
{
    private const string Owner = "owner"; //TODO add to parameters

    public async Task<Account?> GetAccount(int id)
    {
        return await dataAccountService.GetAsync(id);
    }

    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        return await dataAccountService.GetAllAsync().ToListAsync();
    }

    public async Task<InspectAccountModel> InspectAccount(
        int id,
        VisualizationModes mode,
        bool draw = true,
        string color = "",
        string backgroundColor = "",
        int slider = Parameters.ScalingDefault
    )
    {
        var result = new InspectAccountModel();
        var account = await GetAccount(id);

        if (account == null)
            return result;

        foreach (var transaction in account.Transactions)
        {
            // transaction.Amount = Math.Abs(transaction.Amount);
            if (Math.Abs(transaction.Amount) > Parameters.TransferLimit)
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
            .Dispositions.First(x => x.Type.ToLower() == Owner)
            .Customer.Givenname;
        result.CustomerId = account.Dispositions.First(x => x.Type.ToLower() == Owner).CustomerId;
        if (result.NormalTransactions.Count <= 0 || result.TransactionsOverLimit.Count <= 0)
            return result;

        if (!draw)
            return result;

        Color chosenColor = string.IsNullOrEmpty(color) ? Color.Empty : Color.FromName(color);
        Color chosenBackgroundColor = string.IsNullOrEmpty(backgroundColor)
            ? Color.Empty
            : Color.FromName(backgroundColor);
        var scaling = new PlotScalingModel
        {
            HeightScaleFactor = (double)slider / Parameters.ScalingDivider,
            WidthScaleFactor = (double)slider / Parameters.ScalingDivider,
            FontScaleFactor = (double)slider / Parameters.ScalingDivider,
            Color = chosenColor,
            BackgroundColor = chosenBackgroundColor
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
                if (Math.Abs(transaction.Amount) > Parameters.TransferLimit)
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

            var ownerDisposition = account.Dispositions.First(x => x.Type.ToLower() == Owner);

            temp.CustomerName =
                ownerDisposition.Customer.Givenname + " " + ownerDisposition.Customer.Surname;
            temp.CustomerId = ownerDisposition.CustomerId;
        }

        return accountsToInspect;
    }
}
