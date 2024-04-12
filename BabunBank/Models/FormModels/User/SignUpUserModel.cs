using DataAccessLibrary.Data;

namespace BabunBank.Models.FormModels.User;

public class SignUpUserModel
{
    public string UserName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public bool EmailConfirmed { get; init; } = true;

    public UserRole UserRole { get; init; }
}
