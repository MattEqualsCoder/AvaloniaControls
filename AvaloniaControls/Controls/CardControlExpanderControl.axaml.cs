using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaControls.Controls;

public partial class CardControlExpanderControl : UserControl
{
    public CardControlExpanderControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<CardControl, string>(
        nameof(Text), defaultValue: "Expand");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsContentVisibleProperty = AvaloniaProperty.Register<CardControl, bool>(
        nameof(IsContentVisible), defaultValue: true);

    public bool IsContentVisible
    {
        get => GetValue(IsContentVisibleProperty);
        set => SetValue(IsContentVisibleProperty, value);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var parent = Parent;
        while (parent != null && parent is not CardControl)
        {
            parent = parent.Parent;
        }

        if (parent == null)
        {
            return;
        }

        var expander = (CardControl)parent;
        expander.IsContentVisible = !expander.IsContentVisible;
        IsContentVisible = expander.IsContentVisible;
    }
}