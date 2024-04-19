using DataAccessLibrary.Data;

namespace DetectMoneyLaundering.Models;

public class InspectAccountModel
{
    public List<Transaction> TransactionsOverLimit { get; init; } = new List<Transaction>();
    public List<Transaction> NormalTransactions { get; init; } = new List<Transaction>();
    public int TotalNumberOfTransactions { get; set; }
    public string CustomerName { get; set; } = null!;
    public int CustomerId { get; set; }
}
