using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaControls.Controls;

public partial class HeaderFooter : ContentControl
{
    public static readonly StyledProperty<Thickness> BorderSizeProperty = AvaloniaProperty.Register<HeaderFooter, Thickness>(
        nameof(BorderSize), defaultValue: new Thickness(0, 3, 0, 0));
    
    public new static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<HeaderFooter, IBrush>(
        nameof(Background), defaultValue: Brushes.Transparent);

    public Thickness BorderSize
    {
        get => GetValue(BorderSizeProperty);
        set => SetValue(BorderSizeProperty, value);
    }
    
    public new IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

}