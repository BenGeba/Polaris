using Avalonia;
using Avalonia.Controls;

namespace Polaris.Ui.Controls;

/// <summary>
/// Content-area header: title + optional subtitle on the left,
/// the control's Content (actions) docked right. Themed in Styles/Polaris.axaml.
/// </summary>
public class TopBar : ContentControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<TopBar, string?>(nameof(Title));

    public static readonly StyledProperty<string?> SubtitleProperty =
        AvaloniaProperty.Register<TopBar, string?>(nameof(Subtitle));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
}
