namespace DataAccessLibrary.Data;

public class Transaction
{
    public int TransactionId { get; init; }

    public int AccountId { get; set; }

    public DateOnly Date { get; set; } //Change this to datetime TODO

    public string Type { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public string? Symbol { get; set; }

    public string? Bank { get; set; }

    public string? Account { get; set; }

    public virtual Account AccountNavigation { get; set; } = null!;
}
