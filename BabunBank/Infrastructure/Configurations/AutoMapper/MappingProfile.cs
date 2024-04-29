using AutoMapper;
using BabunBank.Models.FormModels.Customer;
using BabunBank.Models.FormModels.CustomerModels;
using BabunBank.Models.FormModels.TransferModels;
using BabunBank.Models.FormModels.User;
using BabunBank.Models.ViewModels.Account;
using BabunBank.Models.ViewModels.Customer;
using BabunBank.Models.ViewModels.Transaction;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Infrastructure.Configurations.AutoMapper;

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
            .ForMember(
                dest => dest.Balance,
                opt => opt.MapFrom(src => src.Transactions.Sum(t => t.Amount))
            )
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Dispositions.First().Type));

        CreateMap<CreateDepositModel, Transaction>().ReverseMap();
        CreateMap<CreateWithdrawalModel, Transaction>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount * -1))
            .ReverseMap();
    }
}
