using AutoMapper;
using BabunBank.Models;
using BabunBank.Models.FormModels.Cashier;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Models.FormModels.User;
using BabunBank.Models.ViewModels.Account;
using BabunBank.Models.ViewModels.Customer;
using BabunBank.Models.ViewModels.Transaction;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Configurations.AutoMapperConfiguration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountViewModel>().ReverseMap();
        CreateMap<Customer, CustomerViewModel>();
        CreateMap<Transaction, TransactionViewModel>();
        CreateMap<SignUpCustomerModel, Customer>();

        CreateMap<IdentityUser, SignUpUserModel>();
        CreateMap<EditCustomerModel, CustomerViewModel>();
        CreateMap<CustomerViewModel, EditCustomerModel>();

        CreateMap<EditCustomerModel, Customer>();
        CreateMap<Customer, EditCustomerModel>();

        //TODO practice these
        CreateMap<Customer, CustomerViewModel>()
            .ForMember(
                x => x.CustomerAccounts,
                x => x.MapFrom(c => c.Dispositions.Select(d => d.Account))
            );

        CreateMap<Account, AccountViewModel>()
            .ForMember(x => x.Transactions, x => x.MapFrom(a => a.Transactions));
    }
}
