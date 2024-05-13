using AutoMapper;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Models.FormModels.IdentityUser;
using BabunBank.Models.FormModels.Transactions;
using BabunBank.Models.ViewModels.Account;
using BabunBank.Models.ViewModels.Customer;
using BabunBank.Models.ViewModels.Transaction;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace BabunBank.Infrastructure.Configurations.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureIdentityUserMaps();
        ConfigureCustomerMaps();
        ConfigureAccountMaps();
        ConfigureTransactionMaps();
    }

    private void ConfigureAccountMaps()
    {
        CreateMap<Account, AccountViewModel>().ReverseMap();
        CreateMap<Account, AccountViewModel>()
            .ForMember(
                dest => dest.Balance,
                opt => opt.MapFrom(src => src.Transactions.Sum(t => t.Amount))
            )
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Dispositions.First().Type));
    }

    private void ConfigureIdentityUserMaps()
    {
        CreateMap<IdentityUser, SignUpIdentityUserModel>();
    }

    private void ConfigureCustomerMaps()
    {
        CreateMap<SignUpCustomerModel, Customer>();
        CreateMap<Customer, CustomerViewModel>();
        CreateMap<EditCustomerModel, CustomerViewModel>().ReverseMap();
        CreateMap<EditCustomerModel, Customer>().ReverseMap();

        //TODO practice these
        CreateMap<Customer, CustomerViewModel>()
            .ForMember(
                x => x.CustomerAccounts,
                x => x.MapFrom(c => c.Dispositions.Select(d => d.Account))
            );
    }

    private void ConfigureTransactionMaps()
    {
        CreateMap<Transaction, TransactionViewModel>();
        CreateMap<CreateDepositModel, Transaction>().ReverseMap();
        CreateMap<CreateWithdrawalModel, Transaction>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount * -1))
            .ReverseMap();
    }
}
