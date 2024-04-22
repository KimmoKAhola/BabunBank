using AutoMapper;
using BabunBank.Models.FormModels.AdModels;
using Babun_API.Models;

namespace Babun_API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ads, ViewAdModel>().ReverseMap();
        CreateMap<Ads, CreateAdModel>().ReverseMap();
        CreateMap<Ads, EditAdModel>().ReverseMap();
    }
}
