using System.Collections;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataLandingPageService(BankAppDataContext dbContext)
{
    public async Task<IEnumerable<Account>> GetLandingPageQuery()
    {
        var data = await dbContext.Accounts
            .Include(x => x.Dispositions)
            .ThenInclude(x => x.Customer).ToListAsync();
        
        return data;
    }
}