using BabunBank.Models.ViewModels.Customer;

namespace BabunBank.Models.ViewModels.Cashier;

public class CashierViewModel
{
    public IEnumerable<CustomerViewModel> CustomerViewModels { get; set; }
    // public
}
