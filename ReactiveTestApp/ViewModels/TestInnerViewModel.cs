using AvaloniaControls.Models;
using ReactiveTestApp.Models;

namespace ReactiveTestApp.ViewModels;

[MapsTo(typeof(TestInnerClass))]
public class TestInnerViewModel : ViewModelBase
{
    public string InnerClassString { get; set; } = "";
}