using BabunBank.Models.FormModels.TransferModels;
using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Interfaces;

public interface ITransactionService
{
    Task<bool?> CreateWithdrawalAsync(Transaction transaction);
    Task<bool?> CreateDepositAsync(Transaction transaction);
    Task<bool?> CreateTransferAsync(CreateTransferModel transfer);
}
