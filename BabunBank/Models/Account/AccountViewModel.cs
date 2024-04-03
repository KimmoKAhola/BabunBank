namespace BabunBank.Models.Account;

public class AccountViewModel
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int AccountId { get; set; }
    public DateOnly Created { get; set; }
    public decimal Balance { get; set; }
}