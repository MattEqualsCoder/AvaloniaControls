using System;

namespace AvaloniaControls.Models;

public class EnumValueChangedEventArgs
{
    public EnumValueChangedEventArgs(Enum enumValue)
    {
        EnumValue = enumValue;
    }
        
    public Enum EnumValue { get; set; }

}