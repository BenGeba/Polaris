using System.Collections.Generic;
using Polaris.Ui.Models;

namespace Polaris.Ui.ViewModels;

public partial class AlbumsViewModel : ViewModelBase
{
    // Mock data ported from the design system (AlbumsView.jsx) — mirrors the
    // Immich /albums endpoint.
    private static readonly string[][] Covers =
    [
        ["#7e4e3a", "#3a2419"],
        ["#3a5d8c", "#1a3a5c"],
        ["#2f5c4a", "#0f2d1f"],
        ["#4a3a6c", "#1f1838"],
        ["#6c5d3a", "#38301a"],
        ["#6c3a5d", "#381a30"],
        ["#3a6c4a", "#1a382d"],
        ["#5d6c3a", "#2d3818"],
    ];

    public IReadOnlyList<AlbumCard> Albums { get; }

    public string Subtitle => $"{Albums.Count} Alben";

    public AlbumsViewModel()
    {
        (string Name, int Count, bool Shared)[] defs =
        [
            ("Sommer 2025", 412, false),
            ("Lissabon", 198, false),
            ("Hochzeit Anna & Ben", 1284, true),
            ("Bergurlaub", 256, false),
            ("Stadtwanderungen", 87, false),
            ("Familienfotos", 731, true),
            ("Konzerte 2025", 64, false),
            ("Geburtstage", 142, false),
        ];

        var albums = new List<AlbumCard>(defs.Length);
        for (var i = 0; i < defs.Length; i++)
        {
            albums.Add(new AlbumCard(
                defs[i].Name,
                defs[i].Count,
                MockGradients.Diagonal(Covers[i % Covers.Length][0], Covers[i % Covers.Length][1]),
                MockGradients.Diagonal(Covers[(i + 3) % Covers.Length][0], Covers[(i + 3) % Covers.Length][1]),
                defs[i].Shared));
        }

        Albums = albums;
    }
}
