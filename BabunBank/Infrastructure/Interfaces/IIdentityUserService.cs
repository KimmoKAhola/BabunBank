using BabunBank.Models.FormModels.IdentityUser;
using BabunBank.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace BabunBank.Infrastructure.Interfaces;

public interface IIdentityUserService
{
    Task<IdentityUserViewModel> GetSingleAsync(string id);
    Task<bool> CreateAsync(IdentityUser model); //TODO delete?
    Task<IdentityResult> UpdateAsync(UpdateIdentityUserModel model);
    Task<IEnumerable<IdentityUserViewModel>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
    Task<bool> CheckIfExistsByEmailAsync(string email);
    Task<IdentityResult> UpdatePasswordAsync(string id, UpdateIdentityUserPasswordModel model);
}
