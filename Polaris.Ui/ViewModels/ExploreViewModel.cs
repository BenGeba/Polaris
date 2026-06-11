using System.Collections.Generic;
using Polaris.Ui.Models;

namespace Polaris.Ui.ViewModels;

public class ExploreViewModel : ViewModelBase
{
    // Mock data ported from the design system (ExploreView.jsx) — mirrors the
    // Immich /search/people and /search/places endpoints.
    private static readonly string[][] PersonGradients =
    [
        ["#7E4E8C", "#2563EB"],
        ["#2F5C4A", "#0F2D1F"],
        ["#8A6C4F", "#3A2D1F"],
        ["#3A5D8C", "#1A3A5C"],
        ["#7E4E3A", "#3A2419"],
        ["#4A3A6C", "#1F1838"],
    ];

    private static readonly string[][] PlaceGradients =
    [
        ["#3a5d8c", "#1a3a5c"],
        ["#2f5c4a", "#0f2d1f"],
        ["#7e4e3a", "#3a2419"],
        ["#6c5d3a", "#38301a"],
        ["#6c3a5d", "#381a30"],
        ["#4a3a6c", "#1f1838"],
    ];

    public IReadOnlyList<ExplorePerson> People { get; } = Build(
        [("Anna", 412), ("Ben", 387), ("Clara", 256), ("David", 198), ("Eva", 174), ("Felix", 162)],
        PersonGradients,
        (name, count, brush) => new ExplorePerson(name, count, brush));

    public IReadOnlyList<ExplorePlace> Places { get; } = Build(
        [("Berlin", 1284), ("Hamburg", 723), ("München", 612), ("Stuttgart", 411), ("Lissabon", 198), ("Wien", 162)],
        PlaceGradients,
        (name, count, brush) => new ExplorePlace(name, count, brush));

    public IReadOnlyList<ExploreThing> Things { get; } =
    [
        new("Architektur", 1842), new("Tiere", 731), new("Essen", 624),
        new("Strand", 481), new("Berge", 392), new("Konzerte", 218),
    ];

    public string PeopleSubtitle => $"{People.Count} erkannte Gesichter";

    private static List<T> Build<T>((string Name, int Count)[] defs, string[][] gradients,
        System.Func<string, int, Avalonia.Media.IBrush, T> factory)
    {
        var result = new List<T>(defs.Length);
        for (var i = 0; i < defs.Length; i++)
        {
            var gradient = gradients[i % gradients.Length];
            result.Add(factory(defs[i].Name, defs[i].Count, MockGradients.Diagonal(gradient[0], gradient[1])));
        }

        return result;
    }
}
