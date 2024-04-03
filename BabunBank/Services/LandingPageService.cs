
using BabunBank.Models.LandingPage;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;
namespace BabunBank.Services;

public class LandingPageService
{
    private readonly BankAppDataContext _dbContext;
    public LandingPageService(BankAppDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LandingPageViewModel> GetLandingPageInfo()
    {
        var query = await _dbContext.Accounts
            .Include(x => x.Dispositions)
            .ThenInclude(x => x.Customer).ToListAsync();

        var landingPageViewModel = new LandingPageViewModel
        {
            NumberOfAccounts = query.Count,
            NumberOfCustomers = query.SelectMany(x => x.Dispositions.Select(d => d.CustomerId)).Distinct().Count(),
            TotalAccountBalance = query.Sum(x => x.Balance)
        };

        return landingPageViewModel;
    }
}