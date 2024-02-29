using System.Collections.Generic;
using AvaloniaControls.Models;
using ReactiveTestApp.Models;

namespace ReactiveTestApp.ViewModels;

[MapsTo(typeof(TestOuterClass))]
public class TestOuterViewModel : ViewModelBase
{
    public bool Boolean { get; set; }

    public List<string> ListBoxItems { get; set; } = new();
    
    public TestInnerViewModel? InnerObject { get; set; }

    public List<TestInnerViewModel> InnerObjects { get; set; } = new();
}