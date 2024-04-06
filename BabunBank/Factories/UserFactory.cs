using BabunBank.Models;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public static class UserFactory
{
    public static IdentityUser CreateUser(SignUpModel model)
    {
        try
        {
            var user = new IdentityUser();
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
