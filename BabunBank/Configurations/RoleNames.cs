using DataAccessLibrary.Data;

namespace BabunBank.Configurations;

public static class RoleNames
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Cashier = nameof(UserRole.Cashier);
    public const string Customer = nameof(UserRole.Customer);
}
