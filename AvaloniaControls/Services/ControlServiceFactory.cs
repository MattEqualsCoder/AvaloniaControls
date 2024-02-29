using System;
using AvaloniaControls.ControlServices;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaControls.Services;

public class ControlServiceFactory : IControlServiceFactory
{
    private readonly IServiceProvider _services;

    public ControlServiceFactory(IServiceProvider services)
    {
        _services = services;
        IControlServiceFactory.Instance = this;
    }

    public T GetControlServiceInternal<T>() where T : IControlService
    {
        var service = _services.GetService<T>();
        return service ?? Activator.CreateInstance<T>();
    }
}