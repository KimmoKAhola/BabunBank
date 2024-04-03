using System.Linq.Expressions;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;

public class AccountRepository(BankAppDataContext dbContext) : BaseRepository<Account>(dbContext) 
{
    public override async Task<Account> GetAsync(Expression<Func<Account, bool>> expression)
    {
        try
        {
            var result = await dbContext.Accounts
                .Include(i => i.AccountId)
                .Include(x => x.Dispositions)
                .Include(x => x.Transactions)
                .FirstAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}