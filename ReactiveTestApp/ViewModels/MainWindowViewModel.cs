using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

    [Reactive] public TestEnums TestEnum2 { get; set; } = TestEnums.ValueThree;

    [Reactive, ReactiveLinkedProperties(nameof(NullableBoolComboBoxValueText))] public bool? NullableBoolComboBoxValue { get; set; } = true;

    public string NullableBoolComboBoxValueText => NullableBoolComboBoxValue == null
        ? "null"
        : NullableBoolComboBoxValue.ToString() ?? "Unknown";
    
    [Reactive, ReactiveLinkedProperties(nameof(BoolComboBoxValueText))] public bool BoolComboBoxValue { get; set; }
    public string BoolComboBoxValueText => BoolComboBoxValue.ToString() ?? "Unknown";
    
    [Reactive] public int TimeInSeconds { get; set; }
    
    [Reactive] public int ValueNoScroll { get; set; }
    
    [Reactive] public double NumericTextBoxDouble { get; set; }
    
    [Reactive] public int NumericTextBoxInt { get; set; }
    
    [Reactive] public int NumericTextBoxTime { get; set; }

    [Reactive]
    public List<ComboBoxAndSearchItem> SearchItems { get; set; } =
    [
        new ComboBoxAndSearchItem(1, "Item 1", "Item 1 Description"),
        new ComboBoxAndSearchItem(2, "Option 2", "Option 2 Description"),
        new ComboBoxAndSearchItem(3, "Option 3", "Option 3 Description"),
        new ComboBoxAndSearchItem(4)
    ];

    [Reactive] public List<string> SearchItemsText { get; set; } = [];

    [Reactive] public string SelectedDisplay { get; set; } = "";

    [Reactive] public int SelectedValue { get; set; } = 2;

    [Reactive]
    [ReactiveLinkedProperties(nameof(MessageBoxResultDisplayText))]
    public string? MessageBoxResult { get; set; }

    public List<string> LinkedEventDropdown => ["Option 1", "Option 2", "Option 3"];

    [Reactive]
    [ReactiveLinkedEvent(nameof(OnLinkedEventSelection))]
    public string LinkedEventSelection { get; set; } = "";

#pragma warning disable CS0067 // Mark members as static
    public event EventHandler? OnLinkedEventSelection; 
#pragma warning restore CS0067 // Mark members as static

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