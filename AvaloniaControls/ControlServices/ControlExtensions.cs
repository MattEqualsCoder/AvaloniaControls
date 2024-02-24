using Avalonia.Controls;
using AvaloniaControls.Models;

namespace AvaloniaControls.ControlServices;

public static class ControlExtensions
{
    public static T GetControlService<T>(this Control control) where T : IControlService
    {
        return ControlServiceFactory.GetControlService<T>();
    }
}