using System;

namespace AvaloniaControls.Models;

[AttributeUsage(AttributeTargets.Class)]
public class MapsToAttribute : Attribute
{
    public MapsToAttribute(Type type)
    {
        MapsToType = type;
    }
    
    public Type MapsToType { get; private set; } 
}