using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Polaris.Ui.Models;

namespace Polaris.Ui.ViewModels;

public partial class TimelineViewModel : ViewModelBase
{
    // Mock data ported from the design system (TimelineView.jsx) — replace with
    // Immich timeline buckets once the API layer is wired up.
    private static readonly (string Id, string Label, int Count)[] MonthDefs =
    [
        ("m2026-05", "Mai 2026", 18),
        ("m2026-04", "April 2026", 26),
        ("m2026-03", "März 2026", 14),
        ("m2026-02", "Februar 2026", 22),
        ("m2026-01", "Januar 2026", 12),
        ("m2025-12", "Dezember 2025", 31),
        ("m2025-11", "November 2025", 17),
    ];

    private static readonly string[][] Palettes =
    [
        ["#5a7da8", "#1a3a5c", "#9bb4d4"], // overcast sky
        ["#b88c5e", "#3a2419", "#e5c89a"], // warm interior
        ["#5e8a6a", "#0f2d1f", "#a8c8a8"], // forest
        ["#b89a6a", "#3a2d1f", "#e8d5a8"], // golden hour
        ["#7a5e9a", "#1f1838", "#b8a8d4"], // dusk
        ["#9a5a5e", "#382020", "#d4a8a8"], // sunset
        ["#5e9a8a", "#1a3a30", "#a8d4c4"], // ocean
        ["#7a8a5e", "#2d3818", "#c4d4a8"], // meadow
        ["#5e7a9a", "#1a2238", "#a8b8d4"], // blue hour
        ["#9a5e8a", "#381a30", "#d4a8c4"], // magenta
        ["#9a8a5e", "#38301a", "#d4c4a8"], // sand
        ["#5e9a6a", "#1a382d", "#a8d4b4"], // mint
    ];

    public IReadOnlyList<TimelineMonth> Months { get; }

    public string Stats => "6.214 Fotos · 412 Videos · 87,2 GB";

    [ObservableProperty]
    public partial string ThumbSize { get; set; } = "M";

    [ObservableProperty]
    public partial int SelectedCount { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsThumbOpen))]
    public partial TimelineThumb? OpenedThumb { get; set; }

    [ObservableProperty]
    public partial string? OpenedFileName { get; set; }

    public bool IsThumbOpen => OpenedThumb is not null;

    public bool HasSelection => SelectedCount > 0;

    public double RowHeight => ThumbSize switch
    {
        "S" => 92,
        "L" => 170,
        _ => 130,
    };

    public TimelineViewModel()
    {
        Months = MonthDefs.Select((def, seed) =>
        {
            var (id, label, count) = def;
            var parts = id.Replace("m", "").Split('-');
            return new TimelineMonth(id, label, parts[1], parts[0], GenerateThumbs(count, seed));
        }).ToList();

        Months[0].IsActive = true;
    }

    private static List<TimelineThumb> GenerateThumbs(int count, int seed) =>
        Enumerable.Range(0, count).Select(i =>
        {
            var palette = Palettes[(i + seed * 5) % Palettes.Length];
            var isPortrait = (i + seed) % 7 == 3;
            var isWide = (i + seed) % 11 == 5;
            return new TimelineThumb(
                $"{seed}-{i}",
                MockGradients.Compose(palette, i + seed),
                isPortrait ? 0.7 : isWide ? 1.7 : 1,
                (i + seed) % 13 == 6,
                "0:" + (10 + i * 17 % 50).ToString().PadLeft(2, '0'),
                (i + seed) % 19 == 11);
        }).ToList();

    partial void OnThumbSizeChanged(string value)
    {
        OnPropertyChanged(nameof(RowHeight));
        foreach (var month in Months)
        {
            month.RowHeight = RowHeight;
        }
    }

    partial void OnSelectedCountChanged(int value) => OnPropertyChanged(nameof(HasSelection));

    [RelayCommand]
    private void SetThumbSize(string size) => ThumbSize = size;

    public void HandleThumbClick(TimelineThumb thumb, bool isRangeModifier)
    {
        if (isRangeModifier || HasSelection)
        {
            thumb.IsSelected = !thumb.IsSelected;
            SelectedCount += thumb.IsSelected ? 1 : -1;
        }
        else
        {
            OpenedFileName = $"IMG_{thumb.Id.Replace("-", "")}.HEIC";
            OpenedThumb = thumb;
        }
    }

    [RelayCommand]
    private void ClearSelection()
    {
        foreach (var thumb in Months.SelectMany(m => m.Thumbs).Where(t => t.IsSelected))
        {
            thumb.IsSelected = false;
        }

        SelectedCount = 0;
    }

    [RelayCommand]
    private void CloseDetail() => OpenedThumb = null;

    public void SetActiveMonth(TimelineMonth month)
    {
        foreach (var m in Months)
        {
            m.IsActive = ReferenceEquals(m, month);
        }
    }
}
