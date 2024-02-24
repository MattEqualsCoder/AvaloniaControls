using System;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaControls.ControlServices;

public class ControlServiceFactory
{
    private static ControlServiceFactory? _instance { get; set; }

    private readonly IServiceProvider _services;

    public ControlServiceFactory(IServiceProvider services)
    {
        _services = services;
        _instance = this;
    }

    public static T GetControlService<T>() where T : IControlService
    {
        if (_instance != null)
        {
            var service = _instance._services.GetService<T>();
            if (service != null)
            {
                return service;
            }
        }

        return Activator.CreateInstance<T>();
    }
}