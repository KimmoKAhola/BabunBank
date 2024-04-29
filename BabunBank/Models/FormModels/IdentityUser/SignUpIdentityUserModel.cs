using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Models.FormModels.User;

public class SignUpIdentityUserModel
{
    public string Email { get; init; } = null!;

    [EmailAddress]
    [Compare("Email", ErrorMessage = "Email addresses has to match")]
    public string ConfirmEmail { get; init; } = null!;

    public string Password { get; init; } = null!;

    [Compare("Password", ErrorMessage = "Passwords has to match")]
    public string ConfirmPassword { get; init; } = null!;

    public bool EmailConfirmed { get; init; } = true;

    public UserRole UserRole { get; init; }
}
