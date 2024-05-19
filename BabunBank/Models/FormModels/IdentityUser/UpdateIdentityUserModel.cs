using System.ComponentModel.DataAnnotations;
using BabunBank.Infrastructure.Configurations.CustomAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Models.FormModels.IdentityUser;

public record UpdateIdentityUserModel
{
    [Required]
    public string UserId { get; init; } = null!;

    [Required]
    public string OldEmail { get; init; } = null!;

    [EmailAddress]
    [Compare("OldEmail", ErrorMessage = "Email addresses has to match")]
    public string ConfirmOldEmail { get; init; } = null!;

    [Required]
    public string NewEmail { get; init; } = null!;

    [EmailAddress]
    [Compare("NewEmail", ErrorMessage = "Email addresses has to match")]
    public string ConfirmNewEmail { get; init; } = null!;

    [ConfirmUserRole]
    public UserRole UserRole { get; init; }
}
