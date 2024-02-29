using Avalonia.Controls;
using AvaloniaControls.ControlServices;
using AvaloniaControls.Services;

namespace AvaloniaControls.Extensions;

public static class ControlExtensions
{
    public static T GetControlService<T>(this Control control) where T : IControlService
    {
        return IControlServiceFactory.GetControlService<T>();
    }
}