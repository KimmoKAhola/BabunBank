using System.Collections;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataLandingPageService(BankAppDataContext dbContext)
{
    public async Task<
        IEnumerable<(
            string country,
            int NumberOfAccounts,
            decimal TotalBalancePerCountry,
            int NumberOfCustomers
        )>
    > GetLandingPageQuery()
    {
        var query = await dbContext
            .Dispositions.Include(d => d.Customer)
            .Include(x => x.Customer.Dispositions)
            .ThenInclude(x => x.Account)
            .ThenInclude(x => x.Transactions)
            .GroupBy(x => x.Customer.Country)
            .Select(x => new
            {
                Country = x.Key,
                NumberOfAccounts = x.SelectMany(d => d.Customer.Dispositions)
                    .Select(d => d.Account)
                    .Distinct()
                    .Count(),
                TotalBalancePerCountry = x.SelectMany(d => d.Account.Transactions)
                    .Sum(t => t.Amount),
                NumberOfCustomers = x.Select(d => d.Customer).Distinct().Count()
            })
            .ToListAsync();

        var result = query.Select(x =>
            (
                x.Country,
                TotalAccounts: x.NumberOfAccounts,
                TotalTransactions: x.TotalBalancePerCountry,
                TotalCustomers: x.NumberOfCustomers
            )
        );

        return result;
    }

    public async Task<
        IEnumerable<(
            string Country,
            int CustomerId,
            int AccountId,
            decimal TotalBalance,
            string CustomerName
        )>
    > GetDetailedLandingPageQuery(string country)
    {
        var data = await dbContext.Accounts
            .Include(x => x.Dispositions)
            .ThenInclude(x => x.Customer).ToListAsync();
        
        return data;
    }
}
