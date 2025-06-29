using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Material.Icons;
using Material.Icons.Avalonia;

namespace AvaloniaControls.Controls;

public class CheckedMenuItem : MenuItem
{
    static CheckedMenuItem()
    {
        IsCheckedProperty.Changed.AddClassHandler<CheckedMenuItem>(IsCheckedPropertyChanged);
    }
    
    protected override Type StyleKeyOverride => typeof(MenuItem);

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateIcon();
    }

    private static void IsCheckedPropertyChanged(CheckedMenuItem sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdateIcon();
    }
    
    private void UpdateIcon()
    {
        if (IsChecked != true)
        {
            Icon = null;
        }
        else
        {
            var icon = new MaterialIcon
            {
                Kind = MaterialIconKind.Check
            };
            Icon = icon;
        }
    }
}