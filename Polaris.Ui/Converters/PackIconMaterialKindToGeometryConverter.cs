using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using IconPacks.Avalonia.Core;
using IconPacks.Avalonia.Material;

namespace Polaris.Ui.Converters;

/// <summary>
/// Converts a <see cref="PackIconMaterialKind"/> into a <see cref="StreamGeometry"/> for use with PathIcon.
/// Reads the path data directly from the IconPacks.Avalonia.Material package instead of using its
/// PackIconMaterial control, whose bundled styles are not binary-compatible with Avalonia 12 yet
/// (see https://github.com/MahApps/IconPacks.Avalonia/issues/41).
/// </summary>
public class PackIconMaterialKindToGeometryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PackIconMaterialKind kind
            && PackIconDataFactory<PackIconMaterialKind>.DataIndex.Value is { } dataIndex
            && dataIndex.TryGetValue(kind, out var data)
            && !string.IsNullOrEmpty(data))
        {
            return StreamGeometry.Parse(data);
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
