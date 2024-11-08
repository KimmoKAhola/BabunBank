﻿using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.IdentityUser;
using BabunBank.Models.ViewModels.Admin;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

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

    public async Task<IdentityResult> UpdateAsync(UpdateIdentityUserModel model)
    {
        return await dataIdentityUserService.UpdateEmailAsync(
            model.UserId,
            model.NewEmail,
            model.OldEmail,
            model.UserRole.ToString()
        );
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await dataIdentityUserService.DeleteAsync(id);
    }

    public async Task<bool> CheckIfExistsByEmailAsync(string email)
    {
        return await dataIdentityUserService.CheckUserExistsByEmail(email);
    }

    public async Task<IdentityResult> UpdatePasswordAsync(
        string id,
        UpdateIdentityUserPasswordModel model
    )
    {
        return await dataIdentityUserService.UpdatePasswordAsync(
            id,
            model.OldPassword,
            model.NewPassword
        );
    }
}
