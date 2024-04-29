using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Parameters;

public static class UserRoleNames
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Cashier = nameof(UserRole.Cashier);
    public const string Customer = nameof(UserRole.Customer); //TODO remove this and remove the customer seeding.
}
