using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataAccessLibrary.DataServices;

public class DataAccountService(AccountRepository accountRepository) : IDataService<Account>
{
    public async Task<Account?> GetAsync(int id)
    {
        var result = await accountRepository.GetAsync(x => x.AccountId == id);

        return result;
    }

    public IQueryable<Account> GetAllAsync(string sortOrder, string sortColum)
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

    public IQueryable<Account> GetAllAsync()
    {
        try
        {
            var transferLimit = 15000M;
            var result = accountRepository
                .GetAllAsync()
                .Where(a => a.Transactions.Any(t => Math.Abs(t.Amount) > transferLimit))
                .Include(a => a.Transactions)
                .Include(a => a.Dispositions)
                .ThenInclude(d => d.Customer);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAndCustomersAsync(
        int id,
        int pageNumber,
        int pageSize,
        string q
    )
    {
        try
        {
            var result = await accountRepository
                .GetAllAsync()
                .Where(a => a.AccountId != id)
                .Include(a => a.Dispositions)
                .ThenInclude(d => d.Customer)
                .OrderBy(a => a.AccountId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (q.IsNullOrEmpty())
                return result;

            if (int.TryParse(q, out var val))
            {
                return result.Where(a => a.AccountId == val).ToList();
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool?> CreateAsync(Account model)
    {
        var account = await accountRepository.CreateAsync(model);
        return account != null;
    }

    public async Task<bool?> DeleteAsync(Account model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool?> UpdateAsync(Account model)
    {
        return await accountRepository.UpdateAsync(x => x.AccountId == model.AccountId, model)
            != null;
    }
}
