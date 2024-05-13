using BabunBank.Infrastructure.Configurations.AutoMapper;
using BabunBank.Infrastructure.Configurations.Dependencies;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabunBank
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found."
                );
            builder.Services.AddDbContext<BankAppDataContext>(options =>
                options.UseSqlServer(connectionString)
            );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder
                .Services.AddDefaultIdentity<IdentityUser>(options =>
                    options.SignIn.RequireConfirmedAccount = true
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BankAppDataContext>();

            builder.Services.AddResponseCaching();
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add(
                    "Default30",
                    new CacheProfile
                    {
                        Duration = 30,
                        VaryByQueryKeys = ["country"],
                        Location = ResponseCacheLocation.Any,
                        NoStore = false
                    }
                );
            });

            builder.Services.AddControllersWithViews();

            RepositoryConfiguration.Configure(builder.Services);
            ServiceConfiguration.Configure(builder.Services);
            HttpConfiguration.Configuration(builder.Services, builder.Configuration);

            //Seeding
            builder.Services.AddTransient<DataInitializer>();
            //Automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder
                .Services.AddMvc()
                .AddViewOptions(options =>
                {
                    options.HtmlHelperOptions.ClientValidationEnabled = true;
                });

            var app = builder.Build();

            app.UseMigrationsEndPoint();

            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();
            app.UseResponseCaching();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.MapRazorPages();

            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetService<DataInitializer>()!.SeedData();
            app.Run();
        }
    }
}
