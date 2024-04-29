using AutoMapper;
using BabunBank.Models.FormModels.Ad;
using Babun_API.Models;

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
    }
}
