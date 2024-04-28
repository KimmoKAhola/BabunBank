// See https://aka.ms/new-console-template for more information

using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;
using DataAccessLibrary.Repositories;
using DetectMoneyLaundering;
using DetectMoneyLaundering.Models;
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

var model = await moneyLaunderingService.InspectAccount(2, VisualizationModes.Console);
var list = await moneyLaunderingService.InspectAllAccounts();
var scalingModel = new PlotScalingModel
{
    HeightScaleFactor = 4.0,
    WidthScaleFactor = 4.0,
    FontScaleFactor = 10
};

DataVisualizationService.CreateIndividualPlot(model, VisualizationModes.Console, scalingModel);
DataVisualizationService.CreatePlotForAllTransactions(
    list,
    VisualizationModes.Console,
    scalingModel
);
DataVisualizationService.CreateHistogram(list, VisualizationModes.Console);
