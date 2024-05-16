using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.Repositories;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Interfaces;
using DetectMoneyLaundering.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddDbContext<BankAppDataContext>();
    services.AddScoped<AccountRepository>();
    services.AddScoped<DataAccountService>();
    services.AddScoped<IMoneyLaunderingService, MoneyLaunderingService>();
    services.AddScoped<IMenuService, MenuService>();
});

var serviceProvider = builder.Build().Services;

var menu = serviceProvider.GetRequiredService<IMenuService>();

await menu.Display();
