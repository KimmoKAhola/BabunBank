namespace BabunBank.Models.FormModels.IdentityUser;

public record IdentityUser
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}
