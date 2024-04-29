using AutoMapper;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.ViewModels.Admin;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Services;

public class IdentityUserService(DataIdentityUserService dataIdentityUserService)
    : IIdentityUserService
{
    public async Task<IdentityUserViewModel> GetSingleAsync(string id)
    {
        var result = await dataIdentityUserService.GetAsync(id);

        var user = new IdentityUserViewModel
        {
            UserId = result.User.Id,
            Username = result.User.UserName!,
            Email = result.User.Email!,
            RoleName = result.Role.Name!
        };

        return user;
    }

    public async Task<IEnumerable<IdentityUserViewModel>> GetAllAsync()
    {
        var result = await dataIdentityUserService.GetAll();

        var users = result.Select(x => new IdentityUserViewModel
        {
            UserId = x.User.Id,
            Username = x.User.UserName!,
            Email = x.User.Email!,
            RoleName = x.Role.Name!
        });

        return users;
    }

    //TODO can be deleted?
    public async Task<bool> CreateAsync(IdentityUser model)
    {
        return await dataIdentityUserService.CreateAsync(model);
    }

    public async Task<bool?> DeleteAsync(string id)
    {
        return await dataIdentityUserService.DeleteAsync(id);
    }

    //TODO Can be deleted?
    // public async Task<bool> CheckIfExistsByUsernameAsync(string username)
    // {
    //     return await dataIdentityUserService.CheckUserExistsByUserName(username);
    // }

    public async Task<bool> CheckIfExistsByEmailAsync(string email)
    {
        return await dataIdentityUserService.CheckUserExistsByEmail(email);
    }
}
