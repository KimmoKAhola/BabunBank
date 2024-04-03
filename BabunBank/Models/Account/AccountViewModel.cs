namespace BabunBank.Models.Account;

public class AccountViewModel
{
    public int Id { get; set; }
    public DateOnly Created { get; set; }
    public decimal Balance { get; set; }
}