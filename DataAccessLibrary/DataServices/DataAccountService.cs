using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataAccountService(BankAppDataContext dbContext) : IDataService<Account>
{
    public async Task<Account> GetAsync(int id)
    {
        try
        {
            var result = await dbContext.Accounts.FirstAsync(x => x.AccountId == id);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        try
        {
            var result = await dbContext.Accounts.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}