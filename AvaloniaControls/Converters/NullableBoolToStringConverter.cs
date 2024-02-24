using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaControls.Converters;

public sealed class NullableBoolToStringConverter : IValueConverter
{
    private const string Unspecified = "";
    private const string Yes = "Yes";
    private const string No = "No";
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return Unspecified;

        return (bool)value ? Yes : No;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch ((string?)value)
        {
            case Yes:
                return true;
            case No:
                return false;
            default:
                return null;
        }
    }
    
    public static readonly string[] ItemsSource = new[]
    {
        Unspecified,
        Yes,
        No
    };
}
