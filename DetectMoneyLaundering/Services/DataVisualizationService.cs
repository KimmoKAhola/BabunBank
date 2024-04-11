using DetectMoneyLaundering.Models;
using ScottPlot;

namespace DetectMoneyLaundering.Services;

public static class DataVisualizationService
{
    public static void CreatePlot(InspectAccountModel model)
    {
        Plot myPlot = new();
        Plot myPlot2 = new();
        var length = model.TransactionsOverLimit.Count(x => x.Amount > 15000);
        var length2 = model.NormalTransactions.Count;
        var xs = new DateTime[length];
        var xs2 = new DateTime[length2];
        var ys = model.TransactionsOverLimit.Select(x => x.Amount).ToArray();
        var ys2 = model.NormalTransactions.Select(x => x.Amount).ToArray();
        var counter = 0;
        foreach (var dateForTransaction in model.TransactionsOverLimit)
        {
            xs[counter] = DateTime.Parse(dateForTransaction.Date.ToString());
            counter++;
        }

        counter = 0;
        foreach (var dateForNormalTransaction in model.NormalTransactions)
        {
            xs2[counter] = DateTime.Parse(dateForNormalTransaction.Date.ToString());
            counter++;
        }

        var scatter = myPlot.Add.ScatterPoints(xs, ys);
        scatter.MarkerStyle.Shape = MarkerShape.FilledCircle;
        scatter.MarkerStyle.Size = 6;
        scatter.MarkerStyle.Fill.Color = Color.FromHex("#ef4444");
        myPlot.Axes.DateTimeTicksBottom();
        myPlot.Title($"All suspicious transactions for the customer {model.CustomerName}");
        myPlot.XLabel("Date");
        myPlot.YLabel("Transaction amount in SEK");
        myPlot.SavePng("wwwroot/images/moneylaundering/suscpicious-transactions.png", 800, 800);

        var scatter2 = myPlot2.Add.ScatterPoints(xs2, ys2);
        scatter2 = myPlot2.Add.ScatterPoints(xs2, ys2);
        scatter2.MarkerStyle.Shape = MarkerShape.FilledCircle;
        scatter2.MarkerStyle.Size = 6;
        scatter2.MarkerStyle.Fill.Color = Color.FromHex("#169DE9");
        myPlot2.Axes.DateTimeTicksBottom();
        myPlot2.Title($"All normal transactions for the customer {model.CustomerName}");
        myPlot2.XLabel("Date");
        myPlot2.YLabel("Transaction amount in SEK");
        myPlot2.SavePng("wwwroot/images/moneylaundering/normal-transactions.png", 800, 800);
    }
}
