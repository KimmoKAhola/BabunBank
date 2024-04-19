using DataAccessLibrary.Data;
using DetectMoneyLaundering.Models;
using ScottPlot;

namespace DetectMoneyLaundering.Services;

public static class DataVisualizationService
{
    private static readonly Color BadColor = Color.FromArgb(239, 68, 68);
    private static readonly Color GoodColor = Color.FromArgb(22, 157, 233);
    private const decimal SuspiciousTransactionThreshold = 15000M;
    private const int ImageHeight = 800;
    private const int ImageWidth = 1400;
    private const string YLabel = "Normalized transaction amount in SEK";
    private const string XLabel = "Date";
    private const int Granularity = 10;

    public static void CreateIndividualPlot(InspectAccountModel model, VisualizationModes mode)
    {
        var transactionsOverLimit = model
            .TransactionsOverLimit.Where(x => Math.Abs(x.Amount) > SuspiciousTransactionThreshold)
            .ToArray();
        var normalTransactions = model.NormalTransactions.ToArray();
        var length = transactionsOverLimit.Length;
        var length2 = normalTransactions.Length;

        // Plot Creation
        CreateSuspiciousTransactionsPlot(transactionsOverLimit, length, model.CustomerName, mode);
        CreateNormalTransactionsPlot(normalTransactions, length2, model.CustomerName, mode);

        var percentageOfSuspiciousTransactions = length / (length + (double)length2);

        // Pie chart
        CreatePieChart(percentageOfSuspiciousTransactions, model.CustomerName, mode);
    }

    private static void CreateSuspiciousTransactionsPlot(
        Transaction[] transactions,
        int length,
        string customerName,
        VisualizationModes mode
    )
    {
        Plot myPlot = new();
        var labels = new string[length];
        var xs = new double[length];
        var ys = transactions.Select(x => (double)x.Amount).ToArray();
        int transactionCounter = 0;

        foreach (var dateForTransaction in transactions)
        {
            labels[transactionCounter] = dateForTransaction.Date.ToString();
            xs[transactionCounter] = transactionCounter;
            transactionCounter++;
        }
        var scatter = myPlot.AddScatter(xs, ys, BadColor, 0, 8);
        myPlot.Add(scatter);
        myPlot.Title($"All suspicious transactions for the customer {customerName}");
        myPlot.XLabel(XLabel);
        myPlot.YLabel(YLabel);
        myPlot.XTicks(labels);
        myPlot.XAxis.TickLabelStyle(rotation: 90);

        var filePath = GetFilePath(mode, PlotNames.SuspiciousTransactions.ToString());
        myPlot.SaveFig(filePath, ImageWidth, ImageHeight);
    }

    private static string GetFilePath(VisualizationModes mode, string fileEnding)
    {
        var filePath = mode switch
        {
            VisualizationModes.Web => $"wwwroot/images/moneylaundering/{fileEnding}.png",
            VisualizationModes.Console => $"../../../test/{fileEnding}.png",
            _ => ""
        };

        return filePath;
    }

    private static void CreateNormalTransactionsPlot(
        Transaction[] transactions,
        int length,
        string customerName,
        VisualizationModes mode
    )
    {
        Plot myPlot = new();
        var xs = new double[length];
        var labels = new string[length];
        var ys = transactions.Select(x => (double)x.Amount).ToArray();
        int transactionCounter = 0;

        foreach (var dateForTransaction in transactions)
        {
            if (transactionCounter % Granularity == 0)
                labels[transactionCounter] = dateForTransaction.Date.ToString();
            xs[transactionCounter] = transactionCounter;
            transactionCounter++;
        }

        var scatter = myPlot.AddScatter(xs, ys, GoodColor, 0, 8);
        myPlot.Add(scatter);
        myPlot.Title($"All normal transactions for the customer {customerName}");
        myPlot.XLabel(XLabel);
        myPlot.YLabel(YLabel);
        myPlot.XTicks(labels);
        myPlot.XAxis.TickLabelStyle(rotation: 90);

        var filePath = GetFilePath(mode, PlotNames.NormalTransactions.ToString());
        myPlot.SaveFig(filePath, ImageWidth, ImageHeight);
    }

    private static void CreatePieChart(
        double percentageOfSuspiciousTransactions,
        string customerName,
        VisualizationModes mode
    )
    {
        Plot myPlot = new();
        double[] pieSlices =
        [
            percentageOfSuspiciousTransactions,
            1 - percentageOfSuspiciousTransactions
        ];
        Color[] colors = [BadColor, GoodColor];
        string[] labels =
        [
            $"Suspicious Transactions {percentageOfSuspiciousTransactions:P2}",
            $"Normal Transactions {1 - percentageOfSuspiciousTransactions:P2}"
        ];
        var piePlot = myPlot.AddPie(pieSlices);
        piePlot.SliceLabels = labels;
        piePlot.Explode = true;
        piePlot.DonutSize = 0.6;
        piePlot.SliceFillColors = colors;
        myPlot.Legend(true, Alignment.LowerRight);
        myPlot.Grid(true);
        myPlot.Title($"Percentage overview of transactions for the customer {customerName}");

        var filePath = GetFilePath(mode, PlotNames.PieChart.ToString());

        myPlot.SaveFig(filePath, ImageWidth, ImageHeight);
    }

    public static void CreateGeneralPlot()
    {
        Plot myPlot = new();
        myPlot.SavePng("../../../test/bigplot.png", 800, 800);
    }
}
