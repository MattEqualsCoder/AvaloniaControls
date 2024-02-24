using Avalonia.Controls;
using AvaloniaControls.ControlServices;

namespace TestApplication;

public partial class TestServiceControl : UserControl
{
    private readonly TestService? _testService;
    
    public TestServiceControl()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            _testService = this.GetControlService<TestService>();
            this.Find<TextBox>(nameof(TestTextBox))!.Text = _testService.GetText();
        }
    }
}