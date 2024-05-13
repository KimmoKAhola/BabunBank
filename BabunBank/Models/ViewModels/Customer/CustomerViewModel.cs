using BabunBank.Models.ViewModels.Account;

namespace BabunBank.Models.ViewModels.Customer;

public record CustomerViewModel
{
    public int CustomerId { get; init; }
    public string? Gender { get; init; }
    public string? GivenName { get; init; }
    public string? Surname { get; init; }
    public string? Streetaddress { get; init; }
    public string? City { get; init; }
    public string? Zipcode { get; init; }
    public string? Country { get; init; }
    public DateOnly? BirthDay { get; init; }
    public string? NationalId { get; init; }
    public bool IsDeleted { get; init; }
    public List<AccountViewModel>? CustomerAccounts { get; init; }
}
