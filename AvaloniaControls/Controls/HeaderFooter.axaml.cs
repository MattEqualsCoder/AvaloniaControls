using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaControls.Controls;

public partial class HeaderFooter : ContentControl
{
    public static readonly StyledProperty<Thickness> BorderSizeProperty = AvaloniaProperty.Register<HeaderFooter, Thickness>(
        nameof(BorderSize), defaultValue: new Thickness(0, 3, 0, 0));

    public Thickness BorderSize
    {
        get => GetValue(BorderSizeProperty);
        set => SetValue(BorderSizeProperty, value);
    }

}