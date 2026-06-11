using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Polaris.Ui.Models;

namespace Polaris.Ui.Controls;

/// <summary>
/// Lays out children in justified rows of fixed height (photo-grid style):
/// children are packed left to right by aspect ratio until a row is full,
/// then the row is scaled so its total width exactly fills the panel.
/// The last partial row keeps natural widths. Aspect ratios are read from
/// the child's DataContext (<see cref="TimelineThumb.Ratio"/>), defaulting to 1.
/// </summary>
public class JustifiedRowsPanel : Panel
{
    public static readonly StyledProperty<double> RowHeightProperty =
        AvaloniaProperty.Register<JustifiedRowsPanel, double>(nameof(RowHeight), 130);

    public static readonly StyledProperty<double> GapProperty =
        AvaloniaProperty.Register<JustifiedRowsPanel, double>(nameof(Gap), 4);

    static JustifiedRowsPanel()
    {
        AffectsMeasure<JustifiedRowsPanel>(RowHeightProperty, GapProperty);
    }

    public double RowHeight
    {
        get => GetValue(RowHeightProperty);
        set => SetValue(RowHeightProperty, value);
    }

    public double Gap
    {
        get => GetValue(GapProperty);
        set => SetValue(GapProperty, value);
    }

    private static double GetRatio(Control child) =>
        child.DataContext is TimelineThumb thumb && thumb.Ratio > 0 ? thumb.Ratio : 1;

    private List<List<(Control Child, double Width)>> PackRows(double containerWidth)
    {
        var rows = new List<List<(Control, double)>>();
        var current = new List<Control>();
        double currentRatio = 0;

        foreach (var child in Children)
        {
            current.Add(child);
            currentRatio += GetRatio(child);
            var widthAtRowHeight = currentRatio * RowHeight + (current.Count - 1) * Gap;
            if (widthAtRowHeight >= containerWidth)
            {
                var scale = (containerWidth - (current.Count - 1) * Gap) / (currentRatio * RowHeight);
                rows.Add(current.ConvertAll(c => (c, GetRatio(c) * RowHeight * scale)));
                current = [];
                currentRatio = 0;
            }
        }

        if (current.Count > 0)
        {
            rows.Add(current.ConvertAll(c => (c, GetRatio(c) * RowHeight)));
        }

        return rows;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var width = double.IsInfinity(availableSize.Width) ? 880 : availableSize.Width;
        var rows = PackRows(width);

        foreach (var row in rows)
        {
            foreach (var (child, childWidth) in row)
            {
                child.Measure(new Size(childWidth, RowHeight));
            }
        }

        var height = rows.Count == 0 ? 0 : rows.Count * RowHeight + (rows.Count - 1) * Gap;
        return new Size(width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var rows = PackRows(finalSize.Width);
        double y = 0;

        foreach (var row in rows)
        {
            double x = 0;
            foreach (var (child, childWidth) in row)
            {
                child.Arrange(new Rect(x, y, childWidth, RowHeight));
                x += childWidth + Gap;
            }

            y += RowHeight + Gap;
        }

        return finalSize;
    }
}
