using BabunBank.Data;
using BabunBank.Models;

namespace BabunBank.Factories;

public class UserFactory
{
    public static User Create(SignUpModel model)
    {
        try
        {
            var user = new User()
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