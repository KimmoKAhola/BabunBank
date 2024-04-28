using System.Drawing;

namespace DetectMoneyLaundering.Models;

public record PlotScalingModel
{
    public double HeightScaleFactor { get; init; }
    public double WidthScaleFactor { get; init; }
    public double FontScaleFactor { get; init; }

    public Color Color { get; set; }
}
