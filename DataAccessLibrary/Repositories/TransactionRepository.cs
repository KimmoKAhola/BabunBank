using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repositories;

public class TransactionRepository(BankAppDataContext dbContext)
    : BaseRepository<Transaction>(dbContext)
{
    public override async Task<Transaction?> CreateAsync(Transaction entity)
    {
        try
        {
            entity.Balance += entity.Amount; //Should be sum of transactions
            dbContext.Set<Transaction>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public async Task<bool> CreateTransferAsync(Transaction deposit, Transaction withdrawal)
    {
        await using var databaseTransaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            deposit.Balance += deposit.Amount; //Should be sum of transactions
            dbContext.Set<Transaction>().Add(deposit);

            withdrawal.Balance += withdrawal.Amount; //Should be sum of transactions
            dbContext.Set<Transaction>().Add(withdrawal);

            await dbContext.SaveChangesAsync();
            await databaseTransaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await databaseTransaction.RollbackAsync();
        }

        return false;
    }
}
