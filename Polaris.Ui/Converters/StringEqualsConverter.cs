using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Polaris.Ui.Converters;

/// <summary>
/// True when the bound value's string representation equals the converter
/// parameter. Used to mark the active option of segmented controls.
/// </summary>
public class StringEqualsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => string.Equals(value?.ToString(), parameter?.ToString(), StringComparison.Ordinal);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
