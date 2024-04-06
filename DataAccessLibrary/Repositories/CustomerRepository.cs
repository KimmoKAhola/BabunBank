using System.Linq.Expressions;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;

public class CustomerRepository(BankAppDataContext dbContext) : BaseRepository<Customer>(dbContext)
{
    public override async Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression)
    {
        var customer = await dbContext
            .Customers.Include(x => x.Dispositions)
            .ThenInclude(x => x.Account)
            .ThenInclude(x => x.Transactions)
            .FirstAsync(expression);

        return customer;
    }

    public override async Task<bool> DeleteAsync(Expression<Func<Customer, bool>> expression)
    {
        try
        {
            var customer = await dbContext.Customers.FirstAsync(expression);
            customer.IsDeleted = true;

            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
