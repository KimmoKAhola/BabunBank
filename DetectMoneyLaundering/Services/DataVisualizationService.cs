using DataAccessLibrary.Data;
using DetectMoneyLaundering.Models;
using ScottPlot;

namespace DetectMoneyLaundering.Services;

public static class DataVisualizationService
{
    private static readonly string _goodColor = "#169DE9";
    private static readonly string _badColor = "#ef4444";

    public static void CreateIndividualPlot(InspectAccountModel model)
    {
        var transactionsOverLimit = model
            .TransactionsOverLimit.Where(x => x.Amount > 15000)
            .ToArray();
        var normalTransactions = model.NormalTransactions.ToArray();
        var length = transactionsOverLimit.Length;
        var length2 = normalTransactions.Length;

        // Plot Creation
        CreateSuspiciousTransactionsPlot(transactionsOverLimit, length, model.CustomerName);
        CreateNormalTransactionsPlot(normalTransactions, length2, model.CustomerName);

        var percentageOfSuspiciousTransactions = length / (length + (double)length2);

        // Pie chart
        CreatePieChart(percentageOfSuspiciousTransactions, model.CustomerName);
    }

    private static void CreateSuspiciousTransactionsPlot(
        Transaction[] transactions,
        int length,
        string customerName
    )
    {
        Plot myPlot = new();
        var xs = new DateTime[length];
        var ys = transactions.Select(x => x.Amount).ToArray();
        int transactionCounter = 0;

        foreach (var dateForTransaction in transactions)
        {
            xs2[counter] = DateTime.Parse(dateForNormalTransaction.Date.ToString());
            counter++;
            xs[transactionCounter] = DateTime.Parse(dateForTransaction.Date.ToString());
            transactionCounter++;
        }

        var scatter = myPlot.Add.ScatterPoints(xs, ys);
        scatter.MarkerStyle.Shape = MarkerShape.FilledCircle;
        scatter.MarkerStyle.Size = 6;
        scatter.MarkerStyle.Fill.Color = Color.FromHex(_badColor);
        myPlot.Axes.DateTimeTicksBottom();
        myPlot.Title($"All suspicious transactions for the customer {customerName}");
        myPlot.XLabel("Date");
        myPlot.YLabel("Transaction amount in SEK");
        myPlot.SavePng("wwwroot/images/moneylaundering/suscpicious-transactions.png", 800, 800);
    }

        //Good plot
        var scatter2 = myPlot2.Add.ScatterPoints(xs2, ys2);
        scatter2 = myPlot2.Add.ScatterPoints(xs2, ys2);
        scatter2.MarkerStyle.Shape = MarkerShape.FilledCircle;
        scatter2.MarkerStyle.Size = 6;
        scatter2.MarkerStyle.Fill.Color = Color.FromHex(_goodColor);
        myPlot2.Axes.DateTimeTicksBottom();
        myPlot2.Title($"All normal transactions for the customer {model.CustomerName}");
        myPlot2.XLabel("Date");
        myPlot2.YLabel("Transaction amount in SEK");
        myPlot2.SavePng("wwwroot/images/moneylaundering/normal-transactions.png", 800, 800);

        var percentageOfSuspiciousTransactions = length / (length + (double)length2);

        //Pie chart
        List<PieSlice> pieSlices =
        [
            new PieSlice
            {
                Value = percentageOfSuspiciousTransactions,
                FillColor = Color.FromHex(_badColor),
                Label = $"Suspicious transactions - {percentageOfSuspiciousTransactions:P2}"
            },
            new PieSlice
            {
                Value = 1 - percentageOfSuspiciousTransactions,
                FillColor = Color.FromHex(_goodColor),
                Label = $"Normal transactions - {1 - percentageOfSuspiciousTransactions:P2}"
            }
        ];

        var piePlot = myPlot3.Add.Pie(pieSlices);
        piePlot.DonutFraction = .5;
        piePlot.ExplodeFraction = 0.1;

        myPlot3.ShowLegend();

        myPlot3.Title($"Percentage overview of transactions for the customer {model.CustomerName}");
        myPlot3.SavePng("wwwroot/images/moneylaundering/piechart.png", 800, 800);
    }

    public static void CreateGeneralPlot()
    {
        Plot myPlot = new();

        myPlot.SavePng("wwwroot/images/moneylaundering/bigplot.png", 800, 800);
    }
}
