using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AvaloniaControls.Controls;

public class NumericTextBox : TextBox
{
    protected override Type StyleKeyOverride => typeof(TextBox);

    private string? _previousValidText;
    
    public static readonly StyledProperty<bool> IsIntegerProperty = AvaloniaProperty.Register<NumericTextBox, bool>(
        nameof(IsInteger));

    public bool IsInteger
    {
        get => GetValue(IsIntegerProperty);
        set => SetValue(IsIntegerProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsTimeProperty = AvaloniaProperty.Register<NumericTextBox, bool>(
        nameof(IsTime));
    
    public bool IsTime
    {
        get => GetValue(IsTimeProperty);
        set => SetValue(IsTimeProperty, value);
    }
    
    public static readonly StyledProperty<bool> PositiveOnlyProperty = AvaloniaProperty.Register<NumericTextBox, bool>(
        nameof(IsPositiveOnly));
    
    public bool IsPositiveOnly
    {
        get => GetValue(PositiveOnlyProperty);
        set => SetValue(PositiveOnlyProperty, value);
    }
    
    public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<NumericTextBox, double>(
        nameof(Value), defaultBindingMode:BindingMode.TwoWay);
    
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            Value = 0;
            _previousValidText = Text;
        }
        else if (IsTime)
        {
            var value = StringTimeToInt(Text);
            if (value != null)
            {
                Value = value.Value;
                _previousValidText = Text;
            }
        }
        else if (IsInteger)
        {
            if (int.TryParse(Text, out var value))
            {
                Value = value;
                _previousValidText = Text;
            }
        }
        else
        {
            if (double.TryParse(Text, out var value))
            {
                Value = value;
                _previousValidText = Text;
            }
        }
        
        base.OnKeyUp(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        Text = _previousValidText;
        base.OnLostFocus(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Delete or Key.Back or Key.Tab or Key.Enter or Key.Return:
            case >= Key.D0 and <= Key.D9:
            case >= Key.NumPad0 and <= Key.NumPad9:
                break;
            default:
            {
                if (!IsTime && !IsPositiveOnly && e.Key is Key.Subtract or Key.OemMinus)
                    break;
                if (!IsInteger && !IsTime && e.Key is Key.Decimal or Key.OemPeriod)
                    break;
                if (IsTime && e.Key == Key.OemSemicolon && (e.KeyModifiers & KeyModifiers.Shift) != 0)
                    break;
                e.Handled = true;
                break;
            }
        }

        base.OnKeyDown(e);
    }

    private int? StringTimeToInt(string text)
    {
        if (Text?.Contains(':') != true)
        {
            if (int.TryParse(text, out var toReturn))
                return toReturn;
            return null;
        }

        try
        {
            if (Text.Length == 8)
            {
                return (int)TimeSpan.ParseExact(Text, "hh\\:mm\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }
            else if (Text.Length == 7)
            {
                return (int)TimeSpan.ParseExact(Text, "h\\:mm\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }
            else if (Text.Length == 5)
            {
                return (int)TimeSpan.ParseExact(Text, "mm\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }
            else if (Text.Length == 4)
            {
                return (int)TimeSpan.ParseExact(Text, "m\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}