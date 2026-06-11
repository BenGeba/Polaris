using Avalonia.Media;
using IconPacks.Avalonia.Core;
using IconPacks.Avalonia.Material;

namespace Polaris.Ui.Models;

/// <summary>
/// Static geometries for icons referenced directly in XAML via x:Static
/// (outside of NavigationItem data templates). Same Material source as the
/// PackIconMaterialKindToGeometryConverter.
/// </summary>
public static class Icons
{
    private static Geometry Get(PackIconMaterialKind kind)
    {
        PackIconDataFactory<PackIconMaterialKind>.DataIndex.Value!.TryGetValue(kind, out var data);
        return StreamGeometry.Parse(data!);
    }

    public static Geometry ChevronRight { get; } = Get(PackIconMaterialKind.ChevronRight);
    public static Geometry Settings { get; } = Get(PackIconMaterialKind.CogOutline);
    public static Geometry Logout { get; } = Get(PackIconMaterialKind.Logout);
    public static Geometry Magnify { get; } = Get(PackIconMaterialKind.Magnify);
    public static Geometry Upload { get; } = Get(PackIconMaterialKind.Upload);
    public static Geometry More { get; } = Get(PackIconMaterialKind.DotsHorizontal);
    public static Geometry Close { get; } = Get(PackIconMaterialKind.Close);
    public static Geometry Heart { get; } = Get(PackIconMaterialKind.HeartOutline);
    public static Geometry HeartFilled { get; } = Get(PackIconMaterialKind.Heart);
    public static Geometry Share { get; } = Get(PackIconMaterialKind.ShareVariantOutline);
    public static Geometry Folder { get; } = Get(PackIconMaterialKind.FolderOutline);
    public static Geometry Download { get; } = Get(PackIconMaterialKind.Download);
    public static Geometry Information { get; } = Get(PackIconMaterialKind.InformationOutline);
    public static Geometry Camera { get; } = Get(PackIconMaterialKind.CameraOutline);
    public static Geometry MapMarker { get; } = Get(PackIconMaterialKind.MapMarkerOutline);
    public static Geometry Tag { get; } = Get(PackIconMaterialKind.TagOutline);
    public static Geometry Check { get; } = Get(PackIconMaterialKind.Check);
    public static Geometry Play { get; } = Get(PackIconMaterialKind.Play);
    public static Geometry Archive { get; } = Get(PackIconMaterialKind.ArchiveOutline);
    public static Geometry Trash { get; } = Get(PackIconMaterialKind.TrashCanOutline);
    public static Geometry AccountGroup { get; } = Get(PackIconMaterialKind.AccountGroupOutline);
}
