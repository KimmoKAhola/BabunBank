using DataAccessLibrary.Data;
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
                await InspectThreeDaySlidingAverage();
                break;
            case 5:
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
        var date = CheckLastReportDate();
        var listOfAccountsToInspect = await moneyLaunderingService.InspectAllAccounts(date);
        await using (
            var writer = new StreamWriter(ReportFilePath, append: File.Exists(ReportFilePath))
        )
        {
            await writer.WriteLineAsync(Header);
            await writer.WriteLineAsync($"Results from: {DateOnly.FromDateTime(DateTime.Now)}");
            if (listOfAccountsToInspect.Count == 0)
            {
                await writer.WriteLineAsync("No suspicious transactions for this date.");
            }
            else
            {
                foreach (var account in listOfAccountsToInspect)
                {
                    await writer.WriteLineAsync(
                        $"Customer Id: {account.CustomerId}\t-\tCustomer name: {account.CustomerName}"
                    );
                    foreach (var suspiciousTransaction in account.TransactionsOverLimit)
                    {
                        await writer.WriteLineAsync(
                            $"Transaction Id: {suspiciousTransaction.TransactionId}\t-\tAmount: {suspiciousTransaction.Amount:C2}\t-\tDate: {suspiciousTransaction.Date}"
                        );
                    }
                    await writer.WriteLineAsync();
                }
                await writer.WriteLineAsync(Header);
            }
        }
        Console.Clear();
        Console.WriteLine($"The report has been saved as {ReportFileName}.txt");
    }

    private async Task PlotAllAccounts()
    {
        var date = CheckLastReportDate();
        var listOfAccountsToInspect = await moneyLaunderingService.InspectAllAccounts(date);

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

    private static DateOnly CheckLastReportDate()
    {
        if (File.Exists(LastReportDateFilePath))
        {
            var fileContent = File.ReadAllText(LastReportDateFilePath);
            if (DateOnly.TryParse(fileContent, out var reportDate))
            {
                return reportDate;
            }
        }
        File.WriteAllText(LastReportDateFilePath, DateOnly.FromDateTime(DateTime.Today).ToString());
        return DateOnly.MinValue;
    }

    private async Task InspectThreeDaySlidingAverage()
    {
        var date = CheckLastReportDate();
        var listOfAccountsToInspect = await moneyLaunderingService.InspectAllAccounts(date);

        foreach (var customer in listOfAccountsToInspect)
        {
            var accountsGroupedByDate = customer
                .NormalTransactions.Concat(customer.TransactionsOverLimit)
                .GroupBy(t => t.Date)
                .OrderBy(t => t.Key)
                .ToList();

            var listOfAccountChunks = new List<IEnumerable<Transaction>>();
            for (int i = 0; i < accountsGroupedByDate.Count - NumberOfDaysInSlidingWindow; i++)
            {
                if (
                    accountsGroupedByDate[i].Key.DayNumber + NumberOfDaysInSlidingWindow
                    >= accountsGroupedByDate[i + 1].Key.DayNumber
                )
                {
                    if (
                        accountsGroupedByDate[i].Key.DayNumber + NumberOfDaysInSlidingWindow
                        >= accountsGroupedByDate[i + 2].Key.DayNumber
                    )
                    {
                        listOfAccountChunks.Add(
                            accountsGroupedByDate[i]
                                .Concat(accountsGroupedByDate[i + 1])
                                .Concat(accountsGroupedByDate[i + 2])
                        );
                    }
                    else
                    {
                        listOfAccountChunks.Add(
                            accountsGroupedByDate[i].Concat(accountsGroupedByDate[i + 1])
                        );
                    }
                }
                else
                {
                    listOfAccountChunks.Add(accountsGroupedByDate[i]);
                }
            }

            List<(
                decimal sumOfTransactions,
                int accountId,
                List<int> listOfTransactionIds,
                DateOnly startDate,
                DateOnly endDate
            )> test = [];
            foreach (var chunk in listOfAccountChunks)
            {
                var chunkSum = chunk.Sum(x => Math.Abs(x.Amount));
                var accountId = chunk.Select(x => x.AccountId).First();
                var transactionIds = chunk.Select(x => x.TransactionId).ToList();
                var startDate = chunk.First().Date;
                var endDate = chunk.Last().Date;
                if (chunkSum >= MaximumSumOverThreeDayPeriod)
                {
                    test.Add((chunkSum, accountId, transactionIds, startDate, endDate));
                }
            }

            await using var writer = new StreamWriter(ThreeDayAverageReportFilePath, append: true);
            await writer.WriteLineAsync(Header);
            var counter = 0;
            foreach (var valueTuple in test.Where(_ => test.Count != 0))
            {
                if (counter == 0)
                {
                    await writer.WriteLineAsync(
                        $"Suspicious transactions for the account with id \"{test.First().accountId}\""
                    );
                    counter++;
                }
                await writer.WriteLineAsync(
                    $"\nThe transactions occurred between the dates {valueTuple.startDate} - {valueTuple.endDate} and have exceeded the trigger sum of {MaximumSumOverThreeDayPeriod:C2}"
                        + $"\nTotal sum over this period is: {valueTuple.sumOfTransactions:C2}"
                );
                foreach (var transactionId in valueTuple.listOfTransactionIds)
                {
                    await writer.WriteLineAsync("Transaction Id: " + transactionId);
                }
            }
            await writer.WriteLineAsync();
        }
    }
}
