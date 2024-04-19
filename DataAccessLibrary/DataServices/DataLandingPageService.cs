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
        var queryResult = await dbContext
            .Transactions.Join(
                dbContext.Accounts,
                t => t.AccountId,
                a => a.AccountId,
                (t, a) => new { Transaction = t, Account = a }
            )
            .Join(
                dbContext.Dispositions,
                ta => ta.Account.AccountId,
                d => d.AccountId,
                (ta, d) => new { TransactionWithAccount = ta, Disposition = d }
            )
            .Join(
                dbContext.Customers,
                tad => tad.Disposition.CustomerId,
                c => c.CustomerId,
                (tad, c) => new { TransactionWithAccountAndDisposition = tad, Customer = c }
            )
            .Where(tc =>
                tc.Customer.Country.ToLower() == country
                && tc.TransactionWithAccountAndDisposition.Disposition.Type.ToLower() == "owner"
            )
            .GroupBy(tc => new
            {
                tc.Customer.Country,
                tc.TransactionWithAccountAndDisposition.TransactionWithAccount.Account.AccountId,
                tc.Customer.CustomerId,
                CustomerName = tc.Customer.Givenname + " " + tc.Customer.Surname
            })
            .Select(group => new
            {
                Country = group.Key.Country,
                CustomerId = group.Key.CustomerId,
                AccountId = group.Key.AccountId,
                TotalBalance = group.Sum(x =>
                    x.TransactionWithAccountAndDisposition.TransactionWithAccount.Transaction.Amount
                ),
                CustomerName = group.Key.CustomerName
            })
            .OrderByDescending(grouping => grouping.TotalBalance)
            .Take(10)
            .ToListAsync();

        var result = queryResult.Select(x =>
            (x.Country, x.AccountId, x.CustomerId, x.TotalBalance, x.CustomerName)
        );

        return result;
    }
}
