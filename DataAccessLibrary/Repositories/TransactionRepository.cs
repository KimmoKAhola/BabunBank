using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repositories;

public class TransactionRepository(BankAppDataContext dbContext)
    : BaseRepository<Transaction>(dbContext) { }
