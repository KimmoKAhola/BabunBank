using AutoMapper;
using BabunBank.Models;
using DataAccessLibrary.Data;

namespace BabunBank.Factories;

public static class CustomerFactory
{
    public static Customer Create(CreateCustomerModel model, IMapper mapper)
    {
        try
        {
            var customer = mapper.Map<CreateCustomerModel, Customer>(model);
            return customer; //TODO create an account to this customer. Need to add disposition as well as owner.
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
