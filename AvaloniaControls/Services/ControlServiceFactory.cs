using System;
using System.Linq;
using Avalonia.Controls;
using AvaloniaControls.ControlServices;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaControls.Services;

internal class ControlServiceFactory : IControlServiceFactory
{
    private readonly IServiceProvider _services;

    public ControlServiceFactory(IServiceProvider services)
    {
        _services = services;
        IControlServiceFactory.Instance = this;
    }

    public T GetControlServiceInternal<T>() where T : ControlService
    {
        var service = _services.GetService<T>();
        return service ?? Activator.CreateInstance<T>();
    }

    public object? GetControlServiceInternal(Control control)
    {
        var serviceName = control.GetType().Name + "Service";
        if (!IControlServiceFactory.ControlServiceDictionary.TryGetValue(serviceName, out var serviceType))
        {
            return null;
        }

        var service = _services.GetService(serviceType);

        if (service == null)
        {
            return null;
        }

        (service as ControlService)!.ParentControl = control;
        return service;
    }
}