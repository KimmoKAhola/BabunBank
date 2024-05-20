using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.FormModels.IdentityUser;

public record UpdateIdentityUserPasswordModel
{
    public string Username { get; init; } = null!;
    public string UserId { get; init; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    public string OldPassword { get; init; } = null!;

    [Compare("OldPassword", ErrorMessage = "Passwords has to match")]
    public string ConfirmOldPassword { get; init; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    public string NewPassword { get; init; } = null!;

    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords has to match")]
    public string ConfirmNewPassword { get; init; } = null!;
}
