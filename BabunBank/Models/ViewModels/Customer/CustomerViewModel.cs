using BabunBank.Models.ViewModels.Account;

namespace BabunBank.Models.ViewModels.Customer;

/// <summary>
/// Det ska gå att söka efter kund på namn och stad
/// En lista ska visas med kundnummer, personnummer, namn, adress, stad
/// Paginerat med 50* (dropdown med olika val)
///
/// Det ska gå att se kundens totala saldo för alla konton.
///
/// Lägg till en kontobild där man kan se transaktioner för ett individuellt konto.
/// Kontosida som visar kontonummer och saldo samt en lista med transaktioner i desc order.
/// </summary>
public class CustomerViewModel
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

    public bool IsDeleted { get; init; } //TODO is this needed? Only used for soft deletion
    public List<AccountViewModel>? CustomerAccounts { get; init; }
}
