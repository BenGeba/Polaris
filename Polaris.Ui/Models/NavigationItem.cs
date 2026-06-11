using System;
using IconPacks.Avalonia.Material;

namespace Polaris.Ui.Models;

public record NavigationItem(string Title, PackIconMaterialKind Icon, Type ViewModelType);
