using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Polaris.Ui.Models;

public partial class TimelineThumb(string id, IBrush background, double ratio, bool isVideo, string duration, bool isFavorite)
    : ObservableObject
{
    public string Id { get; } = id;
    public IBrush Background { get; } = background;
    public double Ratio { get; } = ratio;
    public bool IsVideo { get; } = isVideo;
    public string Duration { get; } = duration;
    public bool IsFavorite { get; } = isFavorite;

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}

public partial class TimelineMonth(string id, string label, string monthNumber, string year, IReadOnlyList<TimelineThumb> thumbs)
    : ObservableObject
{
    public string Id { get; } = id;
    public string Label { get; } = label;
    public string MonthNumber { get; } = monthNumber;
    public string Year { get; } = year;
    public IReadOnlyList<TimelineThumb> Thumbs { get; } = thumbs;
    public string CountText { get; } = $"· {thumbs.Count} Fotos";

    [ObservableProperty]
    public partial bool IsActive { get; set; }

    [ObservableProperty]
    public partial double RowHeight { get; set; } = 130;
}

public partial class SubAlbum(string title, IBrush cover) : ObservableObject
{
    public string Title { get; } = title;
    public IBrush Cover { get; } = cover;

    [ObservableProperty]
    public partial bool IsActive { get; set; }
}

public record ExplorePerson(string Name, int Count, IBrush Avatar)
{
    public string Initial => Name[..1];
}

public record ExplorePlace(string Name, int Count, IBrush Cover)
{
    public string CountText => $"{Count} Fotos";
}

public record ExploreThing(string Name, int Count);

public record AlbumCard(string Name, int Count, IBrush Cover, IBrush BackCover, bool IsShared = false)
{
    public string CountText => $"{Count} Fotos";
}
