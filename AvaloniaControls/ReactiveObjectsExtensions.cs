using System.Collections.Generic;
using System.Linq;
using AvaloniaControls.Models;
using ReactiveUI;

namespace AvaloniaControls;

public static class ReactiveObjectsExtensions
{
    public static void LinkProperties(this ReactiveObject reactiveObject)
    {
        Dictionary<string, string[]> linkedProps = new();
        
        foreach (var property in reactiveObject.GetType().GetProperties())
        {
            if (property.GetCustomAttributes(true).FirstOrDefault(x => x is ReactiveLinkedPropertiesAttribute) is not ReactiveLinkedPropertiesAttribute reactiveLinkedProperties)
            {
                continue;
            }

            linkedProps.Add(property.Name, reactiveLinkedProperties.LinkedAttributes);
        }

        if (!linkedProps.Any())
        {
            return;
        }

        reactiveObject.PropertyChanged += (sender, args) =>
        {
            if (!linkedProps.TryGetValue(args.PropertyName ?? "", out var otherProps))
            {
                return;
            }

            foreach (var otherProp in otherProps)
            {
                reactiveObject.RaisePropertyChanged(otherProp);    
            }
        };
    }
}