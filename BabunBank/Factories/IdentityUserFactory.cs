using BabunBank.Models.FormModels.User;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public static class IdentityUserFactory
{
    public static async Task<IdentityUser> Create(
        SignUpUserModel userModel,
        UserManager<IdentityUser> userManager
    )
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                EmailConfirmed = userModel.EmailConfirmed
            };
            await userManager.CreateAsync(user, userModel.Password);
            await userManager.AddToRolesAsync(user, new[] { userModel.UserRole.ToString() });
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
