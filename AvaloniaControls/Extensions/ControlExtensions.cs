using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input.Platform;
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

    public static Task<string?> GetClipboardAsync(this Control control)
    {
        return TopLevel.GetTopLevel(control)?.Clipboard?.TryGetTextAsync() ?? Task.FromResult<string?>(null);
    }
    
    public static Task SetClipboardAsync(this Control control, string? text)
    {
        return TopLevel.GetTopLevel(control)?.Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
    }
}