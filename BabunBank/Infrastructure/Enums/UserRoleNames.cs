using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Enums;

public static class UserRoleNames
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Cashier = nameof(UserRole.Cashier);
    public const string Customer = nameof(UserRole.Customer);
}
