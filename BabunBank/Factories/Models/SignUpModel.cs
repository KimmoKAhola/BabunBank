using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Models;

/// <summary>
/// TODO is this needed?
/// </summary>
public class SignUpModel
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public bool EmailConfirmed { get; set; } = true;

    [Required]
    public UserRole UserRole { get; set; }
}
