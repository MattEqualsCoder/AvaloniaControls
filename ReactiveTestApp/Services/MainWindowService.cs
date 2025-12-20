using System.Linq;
using AvaloniaControls.Services;
using Microsoft.Extensions.Logging;
using ReactiveTestApp.ViewModels;

namespace ReactiveTestApp.Services;

public class MainWindowService(ILogger<MainWindowService> logger) : ControlService
{
    public MainWindowViewModel Model { get; set; } = new();

    public MainWindowViewModel InitializeModel()
    {
        Model.SearchItemsText = Model.SearchItems.Select(x => x.Display).ToList();
        return Model;
    }

    public void Log(string text)
    {
        logger.LogInformation(text);
    }
}