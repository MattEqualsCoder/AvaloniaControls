using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Material.Icons;
using Material.Icons.Avalonia;

namespace AvaloniaControls.Controls;

public class IconMenuItem : MenuItem
{
    protected override Type StyleKeyOverride => typeof(MenuItem);

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateIcon();
    }

    public static readonly StyledProperty<MaterialIconKind?> IconKindProperty = AvaloniaProperty.Register<IconMenuItem, MaterialIconKind?>(
        nameof(IconKind));

    public MaterialIconKind? IconKind
    {
        get => GetValue(IconKindProperty);
        set
        {
            SetValue(IconKindProperty, value);
            UpdateIcon();
        }
    }

    protected void UpdateIcon()
    {
        if (IconKind == null)
        {
            Icon = null;
        }
        else
        {
            var icon = new MaterialIcon
            {
                Kind = IconKind.Value
            };
            Icon = icon;
        }
    }
}