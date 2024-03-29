using Avalonia;
using Avalonia.Controls;

namespace AvaloniaControls.Controls;

public class ExpanderControl : ContentControl
{
    public static readonly StyledProperty<string> HeaderTextProperty = AvaloniaProperty.Register<CardControl, string>(
        "HeaderText");

    public string HeaderText
    {
        get => GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public static readonly StyledProperty<bool> IsContentVisibleProperty = AvaloniaProperty.Register<CardControl, bool>(
        nameof(IsContentVisible), defaultValue: true);

    public bool IsContentVisible
    {
        get => GetValue(IsContentVisibleProperty);
        set => SetValue(IsContentVisibleProperty, value);
    }
}