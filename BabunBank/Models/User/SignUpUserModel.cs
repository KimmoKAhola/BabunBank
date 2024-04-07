using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Models.User;

public class SignUpUserModel
{
    [Required]
    public string UserName { get; init; } = null!;

    [Required]
    public string Email { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;

    [Required]
    public bool EmailConfirmed { get; init; } = true;

    [Required]
    public UserRole UserRole { get; init; }
}
