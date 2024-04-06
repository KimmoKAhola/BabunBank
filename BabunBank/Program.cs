using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BabunBank.AutoMapperConfiguration;
using BabunBank.Configurations.DependencyConfiguration;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BabunBank
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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

            //Seeding
            builder.Services.AddTransient<DataInitializer>();

            //Automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            //Create this seeding async. Use Task.Run and Wait() So that the main method does not have to be async
            Task.Run(async () =>
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        await scope.ServiceProvider.GetService<DataInitializer>().SeedData();
                    }
                })
                .Wait();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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

            app.Run();
        }
    }
}
