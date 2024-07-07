using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AvaloniaControls.Controls;

public class NumericUpDownTime : NumericUpDown
{
    protected override Type StyleKeyOverride => typeof(NumericUpDown);

    private string? _previousText;
    
    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        _previousText = Text;
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        
        if (_previousText?.Contains(':') != true)
        {
            return;
        }

        try
        {
            if (_previousText.Length == 5)
            {
                Value = (int)TimeSpan.ParseExact(_previousText, "mm\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }
            else if (_previousText.Length == 4)
            {
                Value = (int)TimeSpan.ParseExact(_previousText, "m\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }
        }
        catch
        {
            // Do nothing
        }
    }
}