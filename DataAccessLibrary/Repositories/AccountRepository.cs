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
                .Include(x => x.Dispositions)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Transactions)
                .FirstAsync(expression);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}