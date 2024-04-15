namespace BabunBank.Models.FormModels.User;

public class UserDto
{
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
}
