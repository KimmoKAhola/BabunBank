using BabunBank.Models.Transaction;

namespace BabunBank.Models.Account;

/// <summary>
///
/// </summary>
public class AccountViewModel
{
    public int AccountId { get; init; }
    public DateOnly Created { get; init; }
    public decimal Balance { get; init; }
    public List<TransactionViewModel>? Transactions { get; set; }
}
