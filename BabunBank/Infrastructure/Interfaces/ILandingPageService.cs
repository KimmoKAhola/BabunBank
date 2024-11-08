﻿using BabunBank.Models.ViewModels.LandingPage;

namespace BabunBank.Infrastructure.Interfaces;

public interface ILandingPageService
{
    Task<IEnumerable<LandingPageViewModel>> GetLandingPageInfo();
    Task<IEnumerable<DetailedLandingPageViewModel>> GetDetailedLandingPageInfo(string country);
}
