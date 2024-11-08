﻿using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.DataServices;

public class DataIdentityUserService(
    IdentityUserRepository identityUserRepository,
    UserManager<IdentityUser> userManager
)
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

    public async Task<bool> DeleteAsync(string id)
    {
        return await identityUserRepository.DeleteAsync(x => x.Id == id);
    }

    private async Task<bool> CheckUserExists(string username)
    {
        try
        {
            return await identityUserRepository.ExistsByUserNameAsync(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CheckUserExistsByEmail(string email)
    {
        try
        {
            return await identityUserRepository.ExistsByEmailAsync(email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public async Task<IdentityResult> UpdateEmailAsync(
        string id,
        string newEmail,
        string oldEmail,
        string newRole
    )
    {
        var user = await GetAsync(id);
        if (user.User.Email != oldEmail)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "OldEmail",
                    Description = $"Wrong email. Please enter {user.User.Email}."
                }
            );
        }
        if (await CheckUserExistsByEmail(newEmail) || newEmail == user.User.Email)
        {
            var resultOfEmailChange = await userManager.SetEmailAsync(user.User, newEmail);
            var currentRole = await userManager.GetRolesAsync(user.User);
            await userManager.RemoveFromRolesAsync(user.User, currentRole);
            await userManager.AddToRoleAsync(user.User, newRole);
            await userManager.SetUserNameAsync(user.User, newEmail);
            return resultOfEmailChange;
        }

        var errors = new List<IdentityError>
        {
            new IdentityError { Code = "NewEmail", Description = "Email is already in use." }
        };
        return IdentityResult.Failed(errors.ToArray());
    }

    public async Task<IdentityResult> UpdatePasswordAsync(
        string id,
        string oldPassword,
        string newPassword
    )
    {
        var user = await GetAsync(id);
        var result = await userManager.ChangePasswordAsync(user.User, oldPassword, newPassword);
        return result;
    }
}
