using AutoMapper;
using BabunBank.Models;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Factories;

public static class CustomerFactory
{
    public static Customer Create(CreateCustomerModel model, IMapper mapper)
    {
        try
        {
            var customer = mapper.Map<CreateCustomerModel, Customer>(model);
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
