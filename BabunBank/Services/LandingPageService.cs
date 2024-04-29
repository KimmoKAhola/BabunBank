using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.ViewModels.LandingPage;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

/// <summary>
///     Du ska se antal kunder, antal konton, summan av saldot på alla konton.
/// </summary>
/// <param name="landingPageService"></param>
public class LandingPageService(DataLandingPageService landingPageService) : ILandingPageService
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

    public async Task<IEnumerable<DetailedLandingPageViewModel>> GetDetailedLandingPageInfo(
        string country
    )
    {
        var data = await landingPageService.GetDetailedLandingPageQuery(country);

        var temp = data.Select(x => new DetailedLandingPageViewModel
        {
            CustomerId = x.CustomerId,
            AccountId = x.AccountId,
            Name = x.CustomerName,
            Country = x.Country,
            AccountSum = x.TotalBalance
        });

        return temp;
    }
}
