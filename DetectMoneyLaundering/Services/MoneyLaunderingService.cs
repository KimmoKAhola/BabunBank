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
        // return await dbContext
        //     .Accounts.Include(a => a.Transactions)
        //     .Include(a => a.Dispositions)
        //     .ThenInclude(d => d.Customer)
        //     .FirstOrDefaultAsync(a =>
        //         a.Dispositions.First(d => d.Type == "OWNER").CustomerId == id
        //     );
    }

    public async Task<InspectAccountModel> InspectAccount(int id, VisualizationModes mode)
    {
        var result = new InspectAccountModel();
        var account = await GetAccount(id);

        foreach (var transaction in account.Transactions)
        {
            transaction.Amount = Math.Abs(transaction.Amount);
            if (transaction.Amount > 15000)
            {
                result.TransactionsOverLimit.Add(transaction);
            }
            else
            {
                result.NormalTransactions.Add(transaction);
            }
        }

        result.TotalNumberOfTransactions = account.Transactions.Count();

        result.CustomerName = account
            .Dispositions.First(x => x.Type.ToLower() == "owner")
            .Customer.Givenname;
        result.CustomerId = account.Dispositions.First(x => x.Type.ToLower() == "owner").CustomerId;
        DataVisualizationService.CreateIndividualPlot(result, mode);

        return result;
    }

    public async Task<Test> InspectAllAccounts()
    {
        var result = new Test();
        var allAccounts = await dataAccountService.GetAllAsync().ToListAsync();

        foreach (var account in allAccounts)
        {
            var temp = new InspectAccountModel();
            result.ListOfSusAccounts.Add(temp);
            foreach (var transaction in account.Transactions)
            {
                transaction.Amount = Math.Abs(transaction.Amount);
                if (transaction.Amount > 15000M)
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

        return result;
    }
}
