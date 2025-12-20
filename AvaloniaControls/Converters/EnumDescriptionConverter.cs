using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using AvaloniaControls.Extensions;

namespace AvaloniaControls.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.GetType().IsEnum == true)
        {
            var enumValue = (Enum)value;
            return new EnumDescription()
            {
                Value = enumValue,
                Description = enumValue.GetDescription()
            };
        }
        throw new ArgumentException("Convert:Value must be an enum.");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is EnumDescription enumDescription)
        {
            return enumDescription.Value!;
        }
        throw new ArgumentException("ConvertBack:Value must be an EnumDescription.");
    }
    
    public static ICollection<EnumDescription> GetEnumDescriptions<A>() where A : Enum
    {
        var toReturn = new List<EnumDescription>();
        
        foreach (var enumValue in Enum.GetValues(typeof(A)).Cast<Enum>())
        {
            toReturn.Add(new EnumDescription()
            {
                Value = enumValue,
                Description = enumValue.GetDescription()
            });
        }

        return toReturn;
    }
}

public class EnumDescription
{
    public Enum? Value { get; set; }
    public string? Description { get; set; }
}