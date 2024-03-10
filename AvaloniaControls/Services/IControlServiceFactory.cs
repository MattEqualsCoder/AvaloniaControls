using System;
using System.Collections.Generic;
using Avalonia.Controls;
using AvaloniaControls.ControlServices;

namespace AvaloniaControls.Services;

public interface IControlServiceFactory
{
    protected static IControlServiceFactory? Instance;

    public static Dictionary<string, Type> ControlServiceDictionary = new();

    protected T? GetControlServiceInternal<T>() where T : ControlService;

    protected object? GetControlServiceInternal(Control control);
    
    public static T? GetControlService<T>(Control? control = null) where T : ControlService
    {
        var service = Instance != null ? Instance.GetControlServiceInternal<T>() : Activator.CreateInstance<T>();
        if (service != null)
        {
            service.ParentControl = control;
        }
        return service;
    }
    
    public static object? GetControlService(Control control)
    {
        if (Instance != null)
        {
            return Instance.GetControlServiceInternal(control);
        }

        throw new InvalidOperationException("ControlServiceFactory not initialized");
    }
}