using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Models.FormModels.IdentityUser;

public record UpdateIdentityUserModel
{
    [Required]
    public string UserId { get; init; } = null!;

    [Required]
    public string Email { get; init; } = null!;

    [EmailAddress]
    [Compare("Email", ErrorMessage = "Email addresses has to match")]
    public string ConfirmEmail { get; init; } = null!;

    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")]
    public string Password { get; init; } = null!;

    [Compare("Password", ErrorMessage = "Passwords has to match")]
    public string ConfirmPassword { get; init; } = null!;

    public bool EmailConfirmed { get; init; } = true;

    public UserRole UserRole { get; init; }
}
