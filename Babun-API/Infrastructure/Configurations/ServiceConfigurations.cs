using Babun_API.Data;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Infrastructure.Configurations;

/// <summary>
/// Provides service configurations for the application.
/// </summary>
public static class ServiceConfigurations
{
    /// <summary>
    /// Registers all the necessary services for the application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance to register services to.</param>
    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("apiAppsettings.json", false, true);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var secondConnectionString = builder.Configuration.GetConnectionString(
            "SecondaryConnection"
        );

        builder.Services.AddDbContext<ApiContext>(opt => opt.UseSqlServer(connectionString));
        builder.Services.AddDbContext<BankAppDataContext>(opt =>
            opt.UseSqlServer(connectionString)
        );

        builder
            .Services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BankAppDataContext>();

        builder.Services.AddScoped<AccountRepository>();
        builder.Services.AddScoped<TransactionRepository>();

        builder.Services.AddScoped<DataAccountService>();
        builder.Services.AddScoped<DataTransactionService>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));
    }
}
