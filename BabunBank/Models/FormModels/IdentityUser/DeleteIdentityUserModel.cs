using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.FormModels.IdentityUser;

public record DeleteIdentityUserModel
{
    [Required]
    public string UserId { get; init; } = null!;
    public string Username { get; init; } = null!;

    [Required(ErrorMessage = "Please enter the username.")]
    [Compare("Username", ErrorMessage = "Username has to match")]
    public string ConfirmUsername { get; init; } = null!;
}
