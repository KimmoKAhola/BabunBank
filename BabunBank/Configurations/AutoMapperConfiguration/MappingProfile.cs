﻿using AutoMapper;
using BabunBank.Models;
using BabunBank.Models.Account;
using BabunBank.Models.Admin;
using BabunBank.Models.Customer;
using BabunBank.Models.Transaction;
using BabunBank.Models.User;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Configurations.AutoMapperConfiguration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountViewModel>();
        CreateMap<Customer, CustomerViewModel>();
        CreateMap<Transaction, TransactionViewModel>();
        CreateMap<SignUpCustomerModel, Customer>();
        CreateMap<IdentityUser, IdentityUserViewModel>();
        CreateMap<IdentityUser, SignUpUserModel>();

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
