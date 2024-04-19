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
}
