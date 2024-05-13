namespace DataAccessLibrary.Data;

public class Transaction
{
    public int TransactionId { get; init; }

    public int AccountId { get; init; }

    public DateOnly Date { get; init; }

    public string Type { get; init; } = null!;

    public string Operation { get; init; } = null!;

    public decimal Amount { get; init; }

    public decimal Balance { get; set; }

    public string? Symbol { get; init; }

    public string? Bank { get; init; }

    public string? Account { get; init; }

    public virtual Account AccountNavigation { get; init; } = null!;
}
