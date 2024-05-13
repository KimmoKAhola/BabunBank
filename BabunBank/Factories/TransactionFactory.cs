using AutoMapper;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.Transactions;
using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;

namespace BabunBank.Factories;

public class TransactionFactory : ITransactionFactory
{
    private readonly IMapper _mapper;

    public TransactionFactory(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Transaction CreateDeposit(CreateDepositModel model)
    {
        try
        {
            var transaction = new Transaction();
            return _mapper.Map(model, transaction);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Transaction CreateWithdrawal(CreateWithdrawalModel model)
    {
        try
        {
            var transaction = new Transaction();
            return _mapper.Map(model, transaction);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public CreateTransferModel CreateTransfer(CreateTransferModel model, AccountViewModel receiver)
    {
        try
        {
            return new CreateTransferModel
            {
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
                Amount = model.Amount,
                Type = model.Type,
                BalanceSender = model.BalanceSender,
                BalanceReceiver = receiver.Balance,
                Symbol = model.Symbol,
                Bank = model.Symbol,
                Account = model.Account
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
