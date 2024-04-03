using System.Collections;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataLandingPageService
{
    private readonly BankAppDataContext _dbContext;
    public DataLandingPageService(BankAppDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Account>> GetLandingPageQuery()
    {
        var data = await _dbContext.Accounts
            .Include(x => x.Dispositions)
            .ThenInclude(x => x.Customer).ToListAsync();
        
        return data;
    }
}