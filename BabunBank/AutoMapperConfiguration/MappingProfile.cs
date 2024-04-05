using AutoMapper;
using BabunBank.Models.Account;
using BabunBank.Models.Customer;
using BabunBank.Models.Transaction;
using DataAccessLibrary.Data;

namespace BabunBank.AutoMapperConfiguration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountViewModel>();
        CreateMap<Customer, CustomerViewModel>();
        CreateMap<Transaction, TransactionViewModel>();

        CreateMap<Customer, CustomerViewModel>()
            .ForMember(x => x.CustomerAccounts,
                x => x.MapFrom(c => c.Dispositions.Select(d => d.Account)));

        CreateMap<Account, AccountViewModel>()
            .ForMember(x => x.Transactions,
                x => x.MapFrom(a => a.Transactions));
    }
}