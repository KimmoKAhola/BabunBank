using AutoMapper;
using BabunBank.Models.FormModels.Ad;
using BabunBank.Models.ViewModels.Account;
using BabunBank.Models.ViewModels.Transaction;
using Babun_API.Models;
using DataAccessLibrary.Data;

namespace Babun_API.Infrastructure.Configurations;

/// <summary>
/// Represents a mapping profile for AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Represents a mapping profile for AutoMapper.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Ads, ViewAdModel>().ReverseMap();
        CreateMap<Ads, CreateAdModel>().ReverseMap();
        CreateMap<EditAdModel, Ads>().ReverseMap();

        CreateMap<Account, AccountViewModel>()
            .ForMember(
                dest => dest.Balance,
                opt => opt.MapFrom(src => src.Transactions.Sum(t => t.Amount))
            )
            .ForMember(
                dest => dest.Transactions,
                opt => opt.MapFrom(src => src.Transactions.OrderByDescending(t => t.TransactionId))
            )
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Dispositions.First().Type))
            .ForMember(
                dest => dest.CustomerId,
                opt => opt.MapFrom(src => src.Dispositions.First().CustomerId)
            );

        CreateMap<Transaction, TransactionViewModel>();
    }
}
