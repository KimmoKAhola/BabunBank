using BabunBank.Models;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public class UserFactory
{
    public static Customer Create(CreateCustomerModel model)
    {
        try
        {
            var user = new Customer()
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
