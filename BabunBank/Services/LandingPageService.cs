
using BabunBank.Models.LandingPage;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using Microsoft.EntityFrameworkCore;
namespace BabunBank.Services;

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