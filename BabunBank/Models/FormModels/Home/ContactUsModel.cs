using Microsoft.Build.Framework;

namespace BabunBank.Models.Home;

public class ContactUsModel
{
    [Required]
    public string FirstName { get; init; } = null!;

    [Required]
    public string LastName { get; init; } = null!;

    [Required]
    public string Email { get; init; } = null!;

    [Required]
    public string Message { get; init; } = null!;
}
