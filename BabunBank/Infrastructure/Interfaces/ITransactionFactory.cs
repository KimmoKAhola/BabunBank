using BabunBank.Models.FormModels.TransferModels;
using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Interfaces;

public interface ITransactionFactory
{
    Transaction CreateDeposit(CreateDepositModel model);
    Transaction CreateWithdrawal(CreateWithdrawalModel model);
    CreateTransferModel CreateTransfer(CreateTransferModel model, AccountViewModel receiver);
}
