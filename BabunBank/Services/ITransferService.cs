using DataAccessLibrary.Data;

namespace BabunBank.Services;

public interface ITransferService
{
    public Task<bool> Withdraw(int id, decimal amount);
    public Task<Account?> Deposit(int id, decimal amount);
    public Task<bool> Transfer(int fromAccId, int toAccId, decimal amount);
}
