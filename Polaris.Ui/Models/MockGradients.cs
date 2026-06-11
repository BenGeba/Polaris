using Avalonia;
using Avalonia.Media;

namespace Polaris.Ui.Models;

/// <summary>
/// Gradient brushes used as stand-ins for photo thumbnails and covers,
/// ported from the design system's mock-data generators.
/// </summary>
public static class MockGradients
{
    public static LinearGradientBrush Diagonal(string from, string to) => new()
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Color.Parse(from), 0),
            new GradientStop(Color.Parse(to), 1),
        ],
    };

    /// <summary>Sky-over-ground horizon: c → a → b, top to bottom.</summary>
    public static LinearGradientBrush Horizon(string a, string b, string c) => new()
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Color.Parse(c), 0),
            new GradientStop(Color.Parse(a), 0.55),
            new GradientStop(Color.Parse(b), 1),
        ],
    };

    /// <summary>Radial highlight off-center, suggesting a subject.</summary>
    public static RadialGradientBrush Subject(string a, string b, string c) => new()
    {
        Center = new RelativePoint(0.4, 0.35, RelativeUnit.Relative),
        GradientOrigin = new RelativePoint(0.4, 0.35, RelativeUnit.Relative),
        RadiusX = new RelativeScalar(0.75, RelativeUnit.Relative),
        RadiusY = new RelativeScalar(0.75, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Color.Parse(c), 0),
            new GradientStop(Color.Parse(a), 0.5),
            new GradientStop(Color.Parse(b), 1),
        ],
    };

    /// <summary>Soft top light: c → a, near-vertical.</summary>
    public static LinearGradientBrush TopLight(string a, string c) => new()
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(0.17, 1, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Color.Parse(c), 0),
            new GradientStop(Color.Parse(a), 0.7),
        ],
    };

    /// <summary>Strong shallow diagonal: b → a → c.</summary>
    public static LinearGradientBrush StrongDiagonal(string a, string b, string c) => new()
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(1, 0.36, RelativeUnit.Relative),
        GradientStops =
        [
            new GradientStop(Color.Parse(b), 0),
            new GradientStop(Color.Parse(a), 0.6),
            new GradientStop(Color.Parse(c), 1),
        ],
    };

    public static IBrush Compose(string[] palette, int index)
    {
        var (a, b, c) = (palette[0], palette[1], palette[2]);
        return (index % 5) switch
        {
            0 => Horizon(a, b, c),
            1 => Diagonal(a, b),
            2 => Subject(a, b, c),
            3 => TopLight(a, c),
            _ => StrongDiagonal(a, b, c),
        };
    }
}
