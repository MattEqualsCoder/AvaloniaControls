using System;

namespace AvaloniaControls.Models;

[AttributeUsage(AttributeTargets.Property)]
public class ReactiveLinkedPropertiesAttribute : Attribute
{
    public ReactiveLinkedPropertiesAttribute(params string[] linkedAttributes)
    {
        LinkedAttributes = linkedAttributes;
    }
    
    public string[] LinkedAttributes { get; private set; }
}