using BabunBank.Models.CustomValidators;
using BabunBank.Services;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering.Services;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Configurations.DependencyConfiguration;

public static class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<PasswordHasher<IdentityUser>>();
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
        services.AddScoped<DropDownService>();
        services.AddScoped<ApiService>();

        //Laundering Services
        services.AddScoped<MoneyLaunderingService>();

        //Fluent Validators
        services.AddScoped<UserValidator>();
        services.AddScoped<ContactUsValidator>();
        services.AddScoped<EditCustomerValidation>();
        services.AddScoped<AdValidator>();

        services.AddHttpClient();
    }
}
