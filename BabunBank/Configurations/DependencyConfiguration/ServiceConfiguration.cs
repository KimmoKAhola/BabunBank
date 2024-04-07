using BabunBank.Services;
using DataAccessLibrary.DataServices;

namespace BabunBank.Configurations.DependencyConfiguration;

public static class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        //Library Services
        services.AddScoped<DataAccountService>();
        services.AddScoped<DataCustomerService>();
        services.AddScoped<DataLandingPageService>();
        services.AddScoped<DataIdentityUserService>();
        services.AddScoped<DataTransactionService>();

        //Services
        services.AddScoped<CustomerService>();
        services.AddScoped<AccountService>();
        services.AddScoped<TransactionService>();
        services.AddScoped<LandingPageService>();
        services.AddScoped<IdentityUserService>();
    }
}
