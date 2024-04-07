using AutoMapper;
using BabunBank.Models;
using BabunBank.Models.Admin;
using BabunBank.Models.User;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public static class IdentityUserFactory
{
    public static async Task<IdentityUser> CreateUser(
        SignUpUserModel userModel,
        UserManager<IdentityUser> userManager
    )
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                EmailConfirmed = userModel.EmailConfirmed,
            };
            await userManager.CreateAsync(user, userModel.Password);

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
