using BabunBank.Models;
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
            var user = new IdentityUser();
            await userManager.CreateAsync(user);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
