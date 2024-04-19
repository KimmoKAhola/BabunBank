namespace BabunBank.Models.ViewModels.LandingPage;

public class LandingPageViewModel
{
    //Antal kunder, antal konton, summan av saldot på alla konton

    public string Country { get; init; } = null!;
    public int NumberOfCustomers { get; init; }
    public decimal TotalBalancePerCountry { get; init; }
    public int NumberOfAccounts { get; init; }
}
