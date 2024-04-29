using BabunBank.Infrastructure.Configurations.CustomValidators;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Services;
using DataAccessLibrary.DataServices;
using DetectMoneyLaundering.Interfaces;
using DetectMoneyLaundering.Services;

namespace BabunBank.Infrastructure.Configurations.Dependencies;

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
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ILandingPageService, LandingPageService>();
        services.AddScoped<IIdentityUserService, IdentityUserService>();
        services.AddScoped<NewsService>();

        //Laundering Services
        services.AddScoped<IMoneyLaunderingService, MoneyLaunderingService>();

        //Fluent Validators
        services.AddScoped<UserValidator>();
        services.AddScoped<ContactUsValidator>();
        services.AddScoped<EditCustomerValidation>();
        services.AddScoped<AdValidator>();
    }
}
