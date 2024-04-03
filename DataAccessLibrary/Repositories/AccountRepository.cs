using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repositories;

public class AccountRepository(BankAppDataContext dbContext) : BaseRepository<Account>(dbContext)
{
    
}