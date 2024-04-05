using BabunBank.Models.Transaction;

namespace BabunBank.Models.Account;

public class AccountViewModel
{
    public int AccountId { get; set; }
    public DateOnly Created { get; set; }
    public decimal Balance { get; set; }

    public List<TransactionViewModel> Transactions { get; set; }
}