using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models;

/// <summary>
/// TODO is this needed?
/// </summary>
public class SignUpModel
{
    [Required]
    public string LoginName { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;
}
