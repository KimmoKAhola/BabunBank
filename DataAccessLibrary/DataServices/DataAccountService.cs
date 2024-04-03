using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataAccountService(AccountRepository accountRepository) : IDataService<Account>
{
    public async Task<Account> GetAsync(int id)
    {
        var result = await accountRepository
            .GetAsync(x => x.AccountId == id);

        return result;
    }

    public IQueryable<Account> GetAll(string sortOrder, string sortColum)
    {
        try
        {
            var result = accountRepository.GetAllAsync();
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateAsync(Account model)
    {
        throw new NotImplementedException();
    }
}