using BabunBank.Infrastructure.Configurations.AutoMapper;
using BabunBank.Infrastructure.Configurations.Dependencies;
using BabunBank.Infrastructure.Configurations.HttpClients;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.ViewModels.ApiBlog;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BabunBank
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            await using var scope = app.Services.CreateAsyncScope();
            await scope.ServiceProvider.GetService<DataInitializer>()!.SeedData();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.MapRazorPages();

            await app.RunAsync();
        }
    }
}
