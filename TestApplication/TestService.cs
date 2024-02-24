using AvaloniaControls.ControlServices;
using Microsoft.Extensions.Logging;

namespace TestApplication;

public class TestService(ILogger<TestServiceControl> logger) : IControlService
{
    public string GetText()
    {
        logger.LogInformation("This is working!");
        return "Hi there!";
    }
}