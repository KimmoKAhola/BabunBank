using BabunBank.Models.ViewModels.Transaction;

namespace BabunBank.Models.ViewModels.Account;

/// <summary>
///
/// </summary>
public class AccountViewModel
{
    public int CustomerId { get; set; }
    public int AccountId { get; init; }
    public DateOnly Created { get; init; }
    public decimal Balance { get; init; }
    public List<TransactionViewModel>? Transactions { get; set; }
}
