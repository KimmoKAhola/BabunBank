using BabunBank.Models.FormModels.User;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace BabunBank.Factories;

public static class IdentityUserFactory
{
    public static async Task<IdentityUser> Create(
        SignUpIdentityUserModel identityUserModel,
        UserManager<IdentityUser> userManager
    )
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = identityUserModel.Email,
                Email = identityUserModel.Email,
                EmailConfirmed = identityUserModel.EmailConfirmed
            };
            await userManager.CreateAsync(user, identityUserModel.Password);
            await userManager.AddToRolesAsync(
                user,
                new[] { identityUserModel.UserRole.ToString() }
            );
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
