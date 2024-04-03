using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repositories;

public class CustomerRepository(BankAppDataContext dbContext) : BaseRepository<Customer>(dbContext)
{
    
}