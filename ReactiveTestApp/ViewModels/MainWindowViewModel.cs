using System;
using System.ComponentModel;
using AvaloniaControls.Controls;
using AvaloniaControls.Models;
using ReactiveUI.Fody.Helpers;

namespace ReactiveTestApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    [Reactive] public string TestString { get; set; } = "Test Value";

    [Reactive] public double Scale { get; set; }

    [Reactive] public string TestPath { get; set; } = "";

    [Reactive] public string FolderPath { get; set; } = "";
    
    [Reactive] public bool Boolean { get; set; }
    
    [Reactive] public bool? NullableBoolean { get; set; }
    
    [Reactive] public TestEnums TestEnum { get; set; }
    
    [Reactive] public TestEnums TestEnum2 { get; set; }

    [Reactive]
    [ReactiveLinkedProperties(nameof(MessageBoxResultDisplayText))]
    public string? MessageBoxResult { get; set; }

    public string MessageBoxResultDisplayText => $"Message Box Result: {MessageBoxResult ?? "N/A"}";
    
    public Func<string, string>? UpdateEnumDescription => s => s + " Updated";

    public Func<Enum?, bool> FilterEnum => a => a != null && (TestEnums)a is TestEnums.ValueOne or TestEnums.ValueTwo;
}

public enum TestEnums
{
    [Description("First Value")]
    ValueOne,
    
    [Description("Second Value")]
    ValueTwo,
    
    [Description("Third Value")]
    ValueThree
}