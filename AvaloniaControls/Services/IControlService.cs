using Avalonia.Controls;

namespace AvaloniaControls.ControlServices;

public abstract class ControlService
{
    public Control? ParentControl { get; set; }
}