using System.Drawing;
using DataAccessLibrary.Data;
using DetectMoneyLaundering.Models;
using ScottPlot;

namespace DetectMoneyLaundering.Services;

public static class DataVisualizationService
{
    private static readonly Color BadColor = Color.FromArgb(239, 68, 68);
    private static readonly Color GoodColor = Color.FromArgb(22, 157, 233);
    private const decimal SuspiciousTransactionThreshold = 15000M;
    private const int DefaultImageHeight = 800;
    private const int DefaultImageWidth = 1400;
    private const string YLabel = "Normalized transaction amount in SEK";
    private const string XLabel = "Date";
    private const int DefaultGranularityValue = 10;
    private const int DefaultFontSize = 12;
    private static int MarkerSize => 8;

    public static void CreateIndividualPlot(
        InspectAccountModel model,
        VisualizationModes mode,
        PlotScalingModel scalingModel
    )
    {
        var transactionsOverLimit = model
            .TransactionsOverLimit.Where(x => Math.Abs(x.Amount) > SuspiciousTransactionThreshold)
            .ToArray();
        var normalTransactions = model.NormalTransactions.ToArray();
        var length = transactionsOverLimit.Length;
        var length2 = normalTransactions.Length;

        if (length2 == 0)
            length2 = 1;

        if (length > 0)
        {
            CreateSuspiciousTransactionsPlot(
                transactionsOverLimit,
                length,
                model.CustomerName,
                mode,
                scalingModel
            );
        }
        CreateNormalTransactionsPlot(
            normalTransactions,
            length2,
            model.CustomerName,
            mode,
            scalingModel
        );

        var percentageOfSuspiciousTransactions = length / (length + (double)length2);

        // Pie chart
        CreatePieChart(percentageOfSuspiciousTransactions, model.CustomerName, mode, scalingModel);
    }

    private static void CreateSuspiciousTransactionsPlot(
        Transaction[] transactions,
        int length,
        string customerName,
        VisualizationModes mode,
        PlotScalingModel scalingModel
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

        if (scalingModel.Color.IsEmpty)
        {
            scalingModel.Color = BadColor;
            scalingModel.BackgroundColor = Color.White;
        }

        var fontSize = (int)(DefaultFontSize * scalingModel.FontScaleFactor);
        var scatter = myPlot.AddScatter(
            xs,
            ys,
            scalingModel.Color,
            0,
            (int)(MarkerSize * scalingModel.FontScaleFactor)
        );
        myPlot.Add(scatter);
        myPlot.Title(
            $"All suspicious transactions for the customer {customerName}",
            size: fontSize
        );
        myPlot.Style(dataBackground: scalingModel.BackgroundColor);
        myPlot.XAxis.Label(XLabel, size: fontSize);
        myPlot.YAxis.Label(YLabel, size: fontSize);
        myPlot.XTicks(labels);
        myPlot.XAxis.TickLabelStyle(fontSize: fontSize, rotation: 90);
        myPlot.YAxis.TickLabelStyle(fontSize: fontSize);
        var filePath = GetFilePath(mode, PlotNames.SuspiciousTransactions.ToString());
        myPlot.SaveFig(
            filePath,
            (int)(DefaultImageWidth * scalingModel.WidthScaleFactor),
            (int)(DefaultImageHeight * scalingModel.HeightScaleFactor)
        );
    }

    private static string GetFilePath(VisualizationModes mode, string fileEnding)
    {
        var filePath = mode switch
        {
            VisualizationModes.Web => $"wwwroot/images/moneylaundering/{fileEnding}.png",
            VisualizationModes.Console => $"../../../Plots/{fileEnding}.png",
            _ => ""
        };

        return filePath;
    }

    private static void CreateNormalTransactionsPlot(
        Transaction[] transactions,
        int length,
        string customerName,
        VisualizationModes mode,
        PlotScalingModel scalingModel
    )
    {
        Plot myPlot = new();
        var xs = new double[length];
        var labels = new string[length];
        var ys = transactions.Select(x => (double)x.Amount).ToArray();
        var transactionCounter = 0;

        foreach (var dateForTransaction in transactions)
        {
            if (transactionCounter % DefaultGranularityValue == 0)
                labels[transactionCounter] = dateForTransaction.Date.ToString();
            xs[transactionCounter] = transactionCounter;
            transactionCounter++;
        }

        if (scalingModel.Color.IsEmpty)
        {
            scalingModel.Color = GoodColor;
            scalingModel.BackgroundColor = Color.White;
        }

        var fontSize = (int)(DefaultFontSize * scalingModel.FontScaleFactor);
        var scatter = myPlot.AddScatter(
            xs,
            ys,
            scalingModel.Color,
            0,
            (int)(8 * scalingModel.FontScaleFactor)
        );
        myPlot.Add(scatter);
        myPlot.Style(dataBackground: scalingModel.BackgroundColor);
        myPlot.Title($"All normal transactions for the customer {customerName}", size: fontSize);
        myPlot.XAxis.Label(XLabel, size: fontSize);
        myPlot.YAxis.Label(YLabel, size: fontSize);
        myPlot.XTicks(labels);
        myPlot.XAxis.TickLabelStyle(fontSize: fontSize, rotation: 90);
        myPlot.YAxis.TickLabelStyle(fontSize: fontSize);

        var filePath = GetFilePath(mode, PlotNames.NormalTransactions.ToString());
        myPlot.SaveFig(
            filePath,
            (int)(DefaultImageWidth * scalingModel.WidthScaleFactor),
            (int)(DefaultImageHeight * scalingModel.HeightScaleFactor)
        );
    }

    private static void CreatePieChart(
        double percentageOfSuspiciousTransactions,
        string customerName,
        VisualizationModes mode,
        PlotScalingModel scalingModel
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
        var fontSize = (int)(DefaultFontSize * scalingModel.FontScaleFactor);
        var piePlot = myPlot.AddPie(pieSlices);
        piePlot.SliceLabels = labels;
        piePlot.Explode = true;
        piePlot.DonutSize = 0.6;
        piePlot.SliceFillColors = colors;
        myPlot.Legend(true, Alignment.LowerRight);
        myPlot.Grid(true);
        myPlot.Title(
            $"Percentage overview of transactions for the customer {customerName}",
            size: fontSize
        );

        var filePath = GetFilePath(mode, PlotNames.PieChart.ToString());

        myPlot.SaveFig(
            filePath,
            (int)(DefaultImageWidth * scalingModel.WidthScaleFactor),
            (int)(DefaultImageHeight * scalingModel.HeightScaleFactor)
        );
    }

    public static void CreatePlotForAllTransactions(
        List<InspectAccountModel> accountsToInspect,
        VisualizationModes mode,
        PlotScalingModel scalingModel
    )
    {
        var maxValue = accountsToInspect.Max(x => x.TransactionsOverLimit.Count);
        var numberOfBins = (int)(10 * scalingModel.HeightScaleFactor);

        var barPlotValues = new double[numberOfBins][];
        var increment = maxValue / (double)numberOfBins;

        for (var i = 0; i < numberOfBins; i++)
        {
            barPlotValues[i] = new double[2];
            barPlotValues[i][0] = increment * (i + 1);
        }

        foreach (var customer in accountsToInspect)
        {
            var badTransactionCount = customer.TransactionsOverLimit.Count;

            var barPlotBinIndex = (int)Math.Ceiling(badTransactionCount / increment) - 1;
            if (barPlotBinIndex < 0)
                barPlotBinIndex = 0;
            barPlotValues[barPlotBinIndex][1]++;
        }

        Plot myPlot = new();
        var positions = new double[numberOfBins];
        var labels = new string[numberOfBins];
        for (int i = 0; i < numberOfBins; i++)
        {
            var bar = myPlot.AddBar(barPlotValues[i][0] - increment / 2, barPlotValues[i][1]);
            bar.BarWidth = increment - 1;
            positions[i] = barPlotValues[i][0] - increment / 2;
            labels[i] = $"{(int)(barPlotValues[i][0] - increment)} - {(int)barPlotValues[i][0]}";
        }

        var fontSize = (int)(scalingModel.FontScaleFactor * DefaultFontSize);
        myPlot.SetAxisLimits(xMin: 0, yMin: 0);
        myPlot.Title(
            "Aggregate bar plot of suspicious transactions for all customers",
            size: fontSize
        );
        myPlot.XTicks(positions, labels);
        myPlot.XAxis.TickLabelStyle(fontSize: fontSize, rotation: 90);
        myPlot.YAxis.TickLabelStyle(fontSize: fontSize);
        myPlot.YAxis.Label("Number of suspicious transactions per span", size: fontSize);
        myPlot.XAxis.Label("Transaction span", size: fontSize);
        var filePath = GetFilePath(mode, PlotNames.Aggregate.ToString());
        myPlot.SaveFig(
            filePath,
            (int)(DefaultImageWidth * scalingModel.WidthScaleFactor),
            (int)(DefaultImageHeight * scalingModel.HeightScaleFactor)
        );
    }

    public static void CreateHistogram(
        List<InspectAccountModel> accountsToInspect,
        VisualizationModes mode
    )
    {
        var plt = new Plot();
        var min = accountsToInspect.Min(x => x.NormalTransactions.Min(t => t.Amount));
        var max = accountsToInspect.Max(x => x.TransactionsOverLimit.Max(t => t.Amount));
        var binCount = accountsToInspect.Count;

        List<double> allTransactions = [];
        var binWidth = 50;
        foreach (var account in accountsToInspect)
        {
            allTransactions.AddRange(
                account.TransactionsOverLimit.Select(transaction =>
                    Math.Round((double)transaction.Amount / binWidth) * binWidth
                )
            );
            allTransactions.AddRange(
                account.NormalTransactions.Select(transaction =>
                    Math.Round((double)transaction.Amount / binWidth) * binWidth
                )
            );
        }

        var histogramHeights = allTransactions.ToArray();

        ScottPlot.Statistics.Histogram hist =
            new(min: (double)min, max: (double)max, binCount: binCount);
        hist.AddRange(histogramHeights);

        plt.AddBar(values: hist.Counts, positions: hist.Bins);
        var filePath = GetFilePath(mode, PlotNames.Histogram.ToString());
        plt.SaveFig(filePath, DefaultImageWidth, DefaultImageHeight);
    }
}
