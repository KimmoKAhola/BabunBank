using AutoMapper;
using BabunBank.Models.Admin;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BabunBank.Services;

public class IdentityUserService(DataIdentityUserService dataIdentityUserService, IMapper mapper)
{
    public async Task<IEnumerable<IdentityUserViewModel>> GetAllAsync()
    {
        var users = await dataIdentityUserService.GetAll().ToListAsync();

        var result = mapper.Map<IEnumerable<IdentityUserViewModel>>(users);

        return result;
    }
}
