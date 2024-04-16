﻿using AutoMapper;
using Babun_API.Models;

namespace Babun_API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ads, ViewAdModel>().ReverseMap();
        CreateMap<Ads, CreateAdModel>().ReverseMap();
    }
}