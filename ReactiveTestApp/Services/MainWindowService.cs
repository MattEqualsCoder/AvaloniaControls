using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using AvaloniaControls.ControlServices;
using Microsoft.Extensions.Logging;
using ReactiveTestApp.Models;
using ReactiveTestApp.ViewModels;

namespace ReactiveTestApp.Services;

public class MainWindowService(ILogger<MainWindowService> logger, IMapper mapper) : ControlService
{
    public MainWindowViewModel Model { get; set; } = new();

    public MainWindowViewModel InitializeModel()
    {
        return Model;
    }

    public void Log(string text)
    {
        logger.LogInformation(text);
    }

    public void TestMapping()
    {
        var firstClass = new TestOuterClass
        {
            Boolean = true,
            ListBoxItems = new List<string>
            {
                "Test1",
                "Test2",
            },
            InnerObject = new TestInnerClass
            {
                InnerClassString = "Inner Class String"
            }
        };
        var firstViewModel = mapper.Map<TestOuterViewModel>(firstClass);
        
        var secondViewModel = new TestOuterViewModel()
        {
            Boolean = true,
            ListBoxItems = new List<string>
            {
                "Test1",
                "Test2",
            },
            InnerObject = new TestInnerViewModel()
            {
                InnerClassString = "Inner Class String"
            }
        };
        var secondClass = mapper.Map<TestOuterClass>(secondViewModel);

        var viewModelMatches = JsonSerializer.Serialize(firstViewModel) == JsonSerializer.Serialize(secondViewModel);
        var classMatches = JsonSerializer.Serialize(firstClass) == JsonSerializer.Serialize(secondClass);

        logger.LogInformation("View Model Matches: {Value}", viewModelMatches ? "Yes" : "No");
        logger.LogInformation("Class Matches: {Value}", classMatches ? "Yes" : "No");
    }
}