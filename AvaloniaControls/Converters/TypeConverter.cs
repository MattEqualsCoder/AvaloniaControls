using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaControls.Converters;

public class TypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.GetType();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}