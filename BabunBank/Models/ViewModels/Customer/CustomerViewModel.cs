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
    public int CustomerId { get; set; }
    public string Gender { get; set; }
    public string GivenName { get; set; }
    public string Surname { get; set; }
    public string Streetaddress { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
    public string Country { get; set; }

    public DateOnly? BirthDay { get; set; }
    public string? NationalId { get; set; }

    public bool IsDeleted { get; set; } //TODO is this needed? Only used for soft deletion
    public List<AccountViewModel> CustomerAccounts { get; set; }
}
