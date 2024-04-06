
using BabunBank.Models.LandingPage;
using DataAccessLibrary.DataServices;
namespace BabunBank.Services;

/// <summary>
/// Du ska se antal kunder, antal konton, summan av saldot på alla konton.
/// </summary>
/// <param name="landingPageService"></param>
public class LandingPageService(DataLandingPageService landingPageService)
{
    public async Task<LandingPageViewModel> GetLandingPageInfo()
    {
        var accountData = await landingPageService.GetLandingPageQuery();

        var viewModel = new LandingPageViewModel
        {
            NumberOfAccounts = accountData.Count(),
            NumberOfCustomers = accountData.SelectMany(a => a.Dispositions).Select(d => d.CustomerId).Distinct().Count(),
            TotalAccountBalance = accountData.Sum(a => a.Balance)
        };

        return viewModel;
    }
}