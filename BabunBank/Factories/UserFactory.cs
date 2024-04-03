using BabunBank.Models;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public class UserFactory
{
    public static IdentityUser Create(SignUpModel model)
    {
        try
        {
            var user = new IdentityUser()
            {
                //TODO add some shit here to make sign up work.
            };

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}