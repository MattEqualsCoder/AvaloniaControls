using AvaloniaControls.Models;

namespace TestApplication;

[MapsTo(typeof(ChildClass))]
public class ChildViewModel : BaseViewModel
{
    public string ChildString { get; set; } = "";
}