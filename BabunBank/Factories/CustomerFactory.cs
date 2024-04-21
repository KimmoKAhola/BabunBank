using AutoMapper;
using BabunBank.Models.FormModels.Customer;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices.Enums;

namespace BabunBank.Factories;

public static class CustomerFactory
{
    public static Customer Create(SignUpCustomerModel model, IMapper mapper)
    {
        try
        {
            var customer = mapper.Map<SignUpCustomerModel, Customer>(model);
            customer.Gender = Enum.GetName(typeof(GenderOptions), model.GenderRole);
            customer.Country = Enum.GetName(typeof(CountryOptions), model.Country);
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
