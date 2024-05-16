using DetectMoneyLaundering.Interfaces;
using DetectMoneyLaundering.Models;

namespace DetectMoneyLaundering.Services;

public class MenuService(IMoneyLaunderingService moneyLaunderingService) : IMenuService
{
    private const int MinimumMenuChoice = 1;
    private const int MaximumMenuChoice = 5;
    private const int HeaderLength = 100;
    private const decimal MaximumSumOverThreeDayPeriod = 22000m;
    private const int NumberOfDaysInSlidingWindow = 2; //2 corresponds to 72 hours.
    private static readonly string Header = new('=', HeaderLength);
    private const string ReportFileName = "MoneyLaunderingReport";
    private const string ReportFilePath = $"../../../{ReportFileName}.txt";
    private const string LastReportDate = "LastReportDate";
    private const string LastReportDateFilePath = $"../../../{LastReportDate}.txt";
    private const string ThreeDayAverageReportFileName = "ThreeDayAverage";
    private const string ThreeDayAverageReportFilePath =
        $"../../../{ThreeDayAverageReportFileName}.txt";

    private static readonly PlotScalingModel StandardPlotScalingModel =
        new()
        {
            HeightScaleFactor = 1.0,
            WidthScaleFactor = 1.0,
            FontScaleFactor = 1
        };

    public async Task Display()
    {
        while (true)
        {
            Console.WriteLine(Header);
            Console.WriteLine("1. Text based money laundering.");
            Console.WriteLine("2. Plot and text based money laundering (might take a long time).");
            Console.WriteLine("3. Inspect an individual customer.");
            Console.WriteLine("4. Three day average.");
            Console.WriteLine("5. Exit.");
            Console.WriteLine(Header);
            var choice = GetUserInput();
            await PerformUserChoice(choice);
        }
    }

    private async Task PerformUserChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                await TextMoneyLaundering();
                break;
            case 2:
                await PlotAllAccounts();
                break;
            case 3:
                await PlotIndividualCustomer();
                break;
            case 4:
                Environment.Exit(0);
                break;
        }
    }

    private static int GetUserInput()
    {
        while (true)
        {
            Console.Write("Choose an option: ");
            if (
                int.TryParse(Console.ReadLine(), out var choice)
                && choice is >= MinimumMenuChoice and <= MaximumMenuChoice
            )
            {
                return choice;
            }

            Console.WriteLine(
                $"Please enter a valid menu choice ({MinimumMenuChoice}-{MaximumMenuChoice})."
            );
        }
    }

    private async Task PlotIndividualCustomer()
    {
        while (true)
        {
            Console.Write("Enter a customer id: ");
            if (
                int.TryParse(Console.ReadLine(), out var customerId)
                && customerId >= MinimumMenuChoice
            )
            {
                try
                {
                    await PlotIndividualCustomer(customerId);
                    Console.Clear();
                    Console.WriteLine(
                        $"The graphs of the customer with id \"{customerId}\" has been saved in the Plots folder."
                    );
                    return;
                }
                catch
                {
                    Console.WriteLine("Please enter a valid customer Id.");
                }
            }
            Console.WriteLine("Please enter a valid customer Id.");
        }
    }

    private async Task TextMoneyLaundering()
    {
        var listOfAccountsToInspect = await moneyLaunderingService.InspectAllAccounts();
        using (var writer = new StreamWriter($"../../../{ReportFileName}.txt"))
        {
            writer.WriteLine(new string('=', HeaderLength));
            writer.WriteLine($"Results from: {DateOnly.FromDateTime(DateTime.Now)}");
            foreach (var account in listOfAccountsToInspect)
            {
                writer.WriteLine(
                    $"Customer Id: {account.CustomerId}\t-\tCustomer name: {account.CustomerName}"
                );
                foreach (var suspiciousTransaction in account.TransactionsOverLimit)
                {
                    writer.WriteLine(
                        $"Transaction Id: {suspiciousTransaction.TransactionId}\t-\tAmount: {suspiciousTransaction.Amount:C2}\t-\tDate: {suspiciousTransaction.Date}"
                    );
                }
                writer.WriteLine();
            }
            writer.WriteLine(new string('=', HeaderLength));
        }
        Console.Clear();
        Console.WriteLine($"The report has been saved as {ReportFileName}.txt");
    }

    private async Task PlotAllAccounts()
    {
        var listOfAccountsToInspect = await moneyLaunderingService.InspectAllAccounts();
        DataVisualizationService.CreatePlotForAllTransactions(
            listOfAccountsToInspect,
            VisualizationModes.Console,
            StandardPlotScalingModel
        );
        DataVisualizationService.CreateHistogram(
            listOfAccountsToInspect,
            VisualizationModes.Console
        );
    }

    private async Task PlotIndividualCustomer(int id)
    {
        var accountToInspect = await moneyLaunderingService.InspectAccount(
            id,
            VisualizationModes.Console
        );
        DataVisualizationService.CreateIndividualPlot(
            accountToInspect,
            VisualizationModes.Console,
            StandardPlotScalingModel
        );
    }
}
