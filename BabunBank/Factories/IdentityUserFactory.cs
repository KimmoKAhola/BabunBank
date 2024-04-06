using AutoMapper;
using BabunBank.Models;
using BabunBank.Models.Admin;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public static class IdentityUserFactory
{
    public static async Task<IdentityUser> CreateUser(
        SignUpModel model,
        UserManager<IdentityUser> userManager
    )
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = model.EmailConfirmed,
            };
            await userManager.CreateAsync(user, model.Password);

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
