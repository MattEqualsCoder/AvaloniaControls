using Avalonia.Controls;

namespace AvaloniaControls.Services;

public abstract class ControlService
{
    public Control? ParentControl { get; set; }
}