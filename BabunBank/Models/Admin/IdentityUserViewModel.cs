using Microsoft.Build.Framework;

namespace BabunBank.Models.Admin;

public class IdentityUserViewModel
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string RoleName { get; set; } = null!;
}
