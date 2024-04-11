using DataAccessLibrary.Data;
using DetectMoneyLaundering.Models;
using Microsoft.EntityFrameworkCore;

namespace DetectMoneyLaundering.Services;

//TODO Rename me to something better
public class MoneyLaunderingService(BankAppDataContext dbContext)
{
    public async Task<Account?> GetAccount(int id)
    {
        return await dbContext
            .Accounts.Include(a => a.Transactions)
            .Include(a => a.Dispositions)
            .ThenInclude(d => d.Customer)
            .FirstOrDefaultAsync(a =>
                a.Dispositions.First(d => d.Type == "OWNER").CustomerId == id
            );
    }

    public async Task<InspectAccountModel> InspectAccount(int id)
    {
        var result = new InspectAccountModel();
        var account = await GetAccount(id);

        foreach (var transaction in account.Transactions)
        {
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

        result.CustomerName = account.Dispositions.First(x => x.Type == "OWNER").Customer.Givenname;
        result.CustomerId = account.Dispositions.First(x => x.Type == "OWNER").CustomerId;
        DataVisualizationService.CreatePlot(result);

        return result;
    }
}
