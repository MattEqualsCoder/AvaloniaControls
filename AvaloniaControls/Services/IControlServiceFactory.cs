using System;
using AvaloniaControls.ControlServices;

namespace AvaloniaControls.Services;

public interface IControlServiceFactory
{
    protected static IControlServiceFactory? Instance;

    protected T GetControlServiceInternal<T>() where T : IControlService;
    
    public static T GetControlService<T>() where T : IControlService
    {
        if (Instance != null)
        {
            return Instance.GetControlServiceInternal<T>();
        }
        
        return Activator.CreateInstance<T>();
    }
}