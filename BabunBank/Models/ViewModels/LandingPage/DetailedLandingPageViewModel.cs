namespace BabunBank.Models.ViewModels.LandingPage;

public record DetailedLandingPageViewModel
{
    public int CustomerId { get; init; }
    public int AccountId { get; init; }
    public string Name { get; init; } = null!;
    public string Country { get; init; } = null!;
    public decimal AccountSum { get; init; }
}
