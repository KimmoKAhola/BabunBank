using AutoMapper;
using BabunBank.Models.Admin;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BabunBank.Services;

public class IdentityUserService(DataIdentityUserService dataIdentityUserService, IMapper mapper)
{
    public async Task<IdentityUserViewModel> GetSingleAsync(string id)
    {
        var result = await dataIdentityUserService.GetAsync(id);

        var user = mapper.Map<IdentityUserViewModel>(result);

        return user;
    }

    public async Task<IEnumerable<IdentityUserViewModel>> GetAllAsync()
    {
        var result = await dataIdentityUserService.GetAll().ToListAsync();

        var users = mapper.Map<IEnumerable<IdentityUserViewModel>>(result);

        return users;
    }

    public async Task<bool> CreateAsync(IdentityUser model)
    {
        return await dataIdentityUserService.CreateAsync(model);
    }

    public async Task<bool?> DeleteAsync(string id)
    {
        return await dataIdentityUserService.DeleteAsync(id);
    }
}
