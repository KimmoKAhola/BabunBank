using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.DataServices;

public class DataIdentityUserService(IdentityUserRepository identityUserRepository)
{
    public async Task<IdentityUser> GetAsync(string id)
    {
        var result = await identityUserRepository.GetAsync(x => x.Id == id);
        return result;
    }

    public IQueryable<IdentityUser> GetAll()
    {
        return identityUserRepository.GetAllAsync().AsQueryable();
    }

    public async Task<bool> CreateAsync(IdentityUser model)
    {
        try
        {
            if (await CheckUserExists(model.Id))
                return false;

            await identityUserRepository.CreateAsync(model);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await identityUserRepository.DeleteAsync(x => x.Id == id);
    }

    public async Task<bool> CheckUserExists(string id)
    {
        try
        {
            return await identityUserRepository.ExistsAsync(x => x.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
