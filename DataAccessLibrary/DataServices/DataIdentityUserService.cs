using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.DataServices;

public class DataIdentityUserService(IdentityUserRepository identityUserRepository)
{
    public async Task<(IdentityUser User, IdentityRole Role)> GetAsync(string id)
    {
        var result = await identityUserRepository.GetAsync(x => x.Id == id);
        return result;
    }

    public Task<IEnumerable<(IdentityUser User, IdentityRole Role)>> GetAll()
    {
        return identityUserRepository.GetAllAsync();
    }

    public async Task<bool> CreateAsync(IdentityUser model)
    {
        try
        {
            if (await CheckUserExists(model.Email))
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

    public async Task<bool?> DeleteAsync(string id)
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

    public async Task<bool> CheckUserExistsByUserName(string username)
    {
        try
        {
            return await identityUserRepository.ExistsAsync(x => x.Id == username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public async Task<bool> CheckUserExistsByEmail(string email)
    {
        try
        {
            return await identityUserRepository.ExistsAsync(x => x.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
