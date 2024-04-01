using BabunBank.Data;
using BabunBank.Models;

namespace BabunBank.Factories;

public class CustomerFactory
{
    public static Customer Create(CreateCustomerModel model)
    {
        try
        {
            var customer = new Customer()
            {
                //TODO insert logic
            };

            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}