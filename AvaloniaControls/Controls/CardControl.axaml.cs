using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace AvaloniaControls.Controls;

public class CardControl : ContentControl
{
    public static readonly StyledProperty<string> HeaderTextProperty = AvaloniaProperty.Register<CardControl, string>(
        "HeaderText");

    public string HeaderText
    {
        get => GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }
    
    public static readonly StyledProperty<bool> CanCollapseProperty = AvaloniaProperty.Register<CardControl, bool>(
        nameof(CanCollapse), defaultValue: false);

    public bool CanCollapse
    {
        get => GetValue(CanCollapseProperty);
        set
        {
            SetValue(CanCollapseProperty, value);
            SetValue(CanNotCollapseProperty, !value);
        }
    }

    public static readonly StyledProperty<bool> CanNotCollapseProperty = AvaloniaProperty.Register<CardControl, bool>(
        nameof(CanNotCollapse), defaultValue: true);

    public bool CanNotCollapse
    {
        get => !GetValue(CanCollapseProperty);
        set
        {
            SetValue(CanCollapseProperty, !value);
            SetValue(CanNotCollapseProperty, value);
        }
    }

    public static readonly StyledProperty<object?> HeaderButtonsProperty = AvaloniaProperty.Register<CardControl, object?>(
        "HeaderButtons");

    public object? HeaderButtons
    {
        get => GetValue(HeaderButtonsProperty);
        set => SetValue(HeaderButtonsProperty, value);
    }

    public static readonly StyledProperty<bool> DisplayHeaderButtonsProperty = AvaloniaProperty.Register<CardControl, bool>(
        "DisplayHeaderButtons");

    public bool DisplayHeaderButtons
    {
        get => GetValue(DisplayHeaderButtonsProperty);
        set => SetValue(DisplayHeaderButtonsProperty, value);
    }

    public static readonly StyledProperty<bool> IsContentVisibleProperty = AvaloniaProperty.Register<CardControl, bool>(
        nameof(IsContentVisible), defaultValue: true);

    public bool IsContentVisible
    {
        get => GetValue(IsContentVisibleProperty);
        set => SetValue(IsContentVisibleProperty, value);
    }

}