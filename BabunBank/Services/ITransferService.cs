namespace BabunBank.Services;

public interface ITransferService
{
    public decimal Withdraw();
    public decimal Deposit();
    public decimal Transfer();
}