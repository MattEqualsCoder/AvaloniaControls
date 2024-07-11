using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AvaloniaControls.Models;
using ReactiveUI;

namespace AvaloniaControls.Extensions;

public static class ReactiveObjectsExtensions
{
    public static void LinkProperties(this ReactiveObject reactiveObject)
    {
        Dictionary<string, string[]> linkedProps = new();
        Dictionary<string, string> linkedEvents = new();

        var events = reactiveObject.GetType().GetEvents().ToDictionary(x => x.Name, x => x);
        foreach (var property in reactiveObject.GetType().GetProperties())
        {
            foreach (var attribute in property.GetCustomAttributes(true)
                         .Where(x => x is ReactiveLinkedPropertiesAttribute or ReactiveLinkedEventAttribute))
            {
                switch (attribute)
                {
                    case ReactiveLinkedPropertiesAttribute linkedProperties:
                        linkedProps.Add(property.Name, linkedProperties.LinkedAttributes);
                        break;
                    case ReactiveLinkedEventAttribute linkedEvent:
                        linkedEvents.Add(property.Name, linkedEvent.LinkedEventName);
                        break;
                }
            }
        }

        if (linkedProps.Count == 0 && linkedEvents.Count == 0)
        {
            return;
        }

        reactiveObject.PropertyChanged += (sender, args) =>
        {
            if (linkedProps.TryGetValue(args.PropertyName ?? "", out var otherProps))
            {
                foreach (var otherProp in otherProps)
                {
                    reactiveObject.RaisePropertyChanged(otherProp);    
                }
            }

            if (linkedEvents.TryGetValue(args.PropertyName ?? "", out var otherEvent))
            {
                var backingField = reactiveObject.GetType().GetField(otherEvent, BindingFlags.Instance | BindingFlags.NonPublic);
                if (backingField == null)
                {
                    return;
                }
                
                if (backingField.GetValue(reactiveObject) is EventHandler eventHandler)
                {
                    eventHandler.Invoke(sender, args);    
                }
                
            }
        };
    }
}