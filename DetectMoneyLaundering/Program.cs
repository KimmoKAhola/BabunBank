﻿// See https://aka.ms/new-console-template for more information

using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.Repositories;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddDbContext<BankAppDataContext>();
    services.AddScoped<AccountRepository>();
    services.AddScoped<DataAccountService>();
    services.AddScoped<MoneyLaunderingService>();
});

var serviceProvider = builder.Build().Services;

var moneyLaunderingService = serviceProvider.GetRequiredService<MoneyLaunderingService>();

await moneyLaunderingService.InspectAccount(2, VisualizationModes.Console);
var list = await moneyLaunderingService.InspectAllAccounts();
DataVisualizationService.CreatePlotForAllTransactions(list, VisualizationModes.Console);
