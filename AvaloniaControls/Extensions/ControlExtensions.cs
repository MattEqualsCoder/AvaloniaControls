using Avalonia.Controls;
using AvaloniaControls.ControlServices;
using AvaloniaControls.Services;

namespace AvaloniaControls.Extensions;

public static class ControlExtensions
{
    public static T? GetControlService<T>(this Control control) where T : ControlService
    {
        return IControlServiceFactory.GetControlService<T>(control);
    }
    
    public static object? GetControlService(this Control control)
    {
        return IControlServiceFactory.GetControlService(control);
    }
}