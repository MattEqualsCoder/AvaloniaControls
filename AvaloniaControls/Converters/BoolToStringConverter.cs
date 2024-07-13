using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaControls.Converters;

public sealed class BoolToStringConverter : IValueConverter
{
    private const string Yes = "Yes";
    private const string No = "No";
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool?)value == true ? Yes : No;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (string?)value switch
        {
            Yes => true,
            _ => false
        };
    }
    
    public static readonly string[] ItemsSource =
    [
        Yes,
        No
    ];
}
