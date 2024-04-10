using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class PlotController : Controller
{
    // GET
    public IActionResult Index()
    {
        double[] x = { 0, 1, 2, 3, 4, 5, 6, 7 };
        double[] y = { 0, 1, 2, 3, 4, 5, 6, 7 };

        ScottPlot.Plot myPlot = new();

        myPlot.Add.Scatter(x, y);

        string filePath = $"temp_plot.png";

        myPlot.SavePng(filePath, 400, 300);
        ViewBag.FilePath = filePath;
        return View();
    }
}
