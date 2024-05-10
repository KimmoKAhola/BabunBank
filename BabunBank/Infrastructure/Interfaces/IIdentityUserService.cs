using BabunBank.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Infrastructure.Interfaces;

public interface IIdentityUserService
{
    Task<IdentityUserViewModel> GetSingleAsync(string id);
    Task<bool> CreateAsync(IdentityUser model); //TODO delete?
    Task<IEnumerable<IdentityUserViewModel>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
    Task<bool> CheckIfExistsByEmailAsync(string email);
}
