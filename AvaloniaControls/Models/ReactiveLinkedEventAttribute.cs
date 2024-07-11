using System;

namespace AvaloniaControls.Models;

[AttributeUsage(AttributeTargets.Property)]
public class ReactiveLinkedEventAttribute(string eventHandlerName) : Attribute
{
    public string LinkedEventName { get; private set; } = eventHandlerName;
}