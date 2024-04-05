using AutoMapper;
using BabunBank.AutoMapperConfiguration;
using BabunBank.Models.Customer;
using BabunBank.Services;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC;

namespace BabunBank
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<BankAppDataContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BankAppDataContext>();
            builder.Services.AddControllersWithViews();
            
            //Repositories
            builder.Services.AddScoped<CustomerRepository>();
            builder.Services.AddScoped<AccountRepository>();
            
            //Library Services
            builder.Services.AddScoped<DataAccountService>();
            builder.Services.AddScoped<DataCustomerService>();
            builder.Services.AddScoped<DataLandingPageService>();
            
            
            
            //Services
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<LandingPageService>();
            
            
            //Seeding
            builder.Services.AddTransient<DataInitializer>();


            //Automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<DataInitializer>().SeedData();
            }

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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}