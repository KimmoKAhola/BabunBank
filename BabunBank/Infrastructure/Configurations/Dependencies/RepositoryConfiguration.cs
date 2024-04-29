using DataAccessLibrary.Repositories;

namespace BabunBank.Infrastructure.Configurations.Dependencies;

public static class RepositoryConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<CustomerRepository>();
        services.AddScoped<AccountRepository>();
        services.AddScoped<TransactionRepository>();
        services.AddScoped<IdentityUserRepository>();
    }
}
