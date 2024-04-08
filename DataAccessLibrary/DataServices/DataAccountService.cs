﻿using DataAccessLibrary.Data;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataServices;

public class DataAccountService(AccountRepository accountRepository) : IDataService<Account>
{
    public async Task<Account?> GetAsync(int id)
    {
        var result = await accountRepository.GetAsync(x => x.AccountId == id);

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

    public async Task<bool?> CreateDepositAsync(Account model)
    {
        var account = await accountRepository.CreateAsync(model);
        return account != null;
    }

    public async Task<bool?> DeleteAsync(Account model)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> UpdateAsync(Account model)
    {
        return await accountRepository.UpdateAsync(x => x.AccountId == model.AccountId, model);
    }

    public async Task Deposit(Account model) { }
}
