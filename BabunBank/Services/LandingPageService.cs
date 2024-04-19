using BabunBank.Models.ViewModels.LandingPage;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

/// <summary>
/// Du ska se antal kunder, antal konton, summan av saldot på alla konton.
/// </summary>
/// <param name="landingPageService"></param>
public class LandingPageService(DataLandingPageService landingPageService)
{
    public async Task<IEnumerable<LandingPageViewModel>> GetLandingPageInfo()
    {
        var accountData = await landingPageService.GetLandingPageQuery();

        var viewModel = accountData.Select(x => new LandingPageViewModel
        {
            Country = x.country,
            NumberOfCustomers = x.NumberOfCustomers,
            TotalBalancePerCountry = x.TotalBalancePerCountry,
            NumberOfAccounts = x.NumberOfAccounts
        });

        return viewModel;
    }
}
