
namespace BabunBank.Models.LandingPage;

public class LandingPageViewModel
{
    //Antal kunder, antal konton, summan av saldot på alla konton
    public int NumberOfAccounts { get; set; }
    public int NumberOfCustomers { get; set; }
    public decimal TotalAccountBalance { get; set; }
}