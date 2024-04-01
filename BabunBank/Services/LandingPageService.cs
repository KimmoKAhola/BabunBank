using BabunBank.Data;
using BabunBank.Models.LandingPage;
using Microsoft.EntityFrameworkCore;
namespace BabunBank.Services;

public class LandingPageService
{
    private readonly ApplicationDbContext _dbContext;
    public LandingPageService(ApplicationDbContext dbContext)
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