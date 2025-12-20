using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AvaloniaControls.Models;
using ReactiveUI.SourceGenerators;

namespace ReactiveTestApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    [Reactive] public partial string TestString { get; set; }

    [Reactive] public partial double Scale { get; set; }

    [Reactive] public partial string TestPath { get; set; } 

    [Reactive] public partial string FolderPath { get; set; }
    
    [Reactive] public partial bool Boolean { get; set; }
    
    [Reactive] public partial bool? NullableBoolean { get; set; }
    
    [Reactive] public partial TestEnums TestEnum { get; set; }

    [Reactive] public partial TestEnums TestEnum2 { get; set; }

    [Reactive, ReactiveLinkedProperties(nameof(NullableBoolComboBoxValueText))]
    public partial bool? NullableBoolComboBoxValue { get; set; }

    public string NullableBoolComboBoxValueText => NullableBoolComboBoxValue == null
        ? "null"
        : NullableBoolComboBoxValue.ToString() ?? "Unknown";
    
    [Reactive, ReactiveLinkedProperties(nameof(BoolComboBoxValueText))] 
    public partial bool BoolComboBoxValue { get; set; }
    
    public string BoolComboBoxValueText => BoolComboBoxValue.ToString() ?? "Unknown";
    
    [Reactive] 
    public partial int TimeInSeconds { get; set; }
    
    [Reactive] 
    public partial int ValueNoScroll { get; set; }
    
    [Reactive] 
    public partial double NumericTextBoxDouble { get; set; }
    
    [Reactive]
    public partial int NumericTextBoxInt { get; set; }
    
    [Reactive]
    public partial int NumericTextBoxTime { get; set; }

    public List<ComboBoxAndSearchItem> SearchItems { get; } =
    [
        new ComboBoxAndSearchItem(1, "Item 1", "Item 1 Description"),
        new ComboBoxAndSearchItem(2, "Option 2", "Option 2 Description"),
        new ComboBoxAndSearchItem(3, "Option 3", "Option 3 Description"),
        new ComboBoxAndSearchItem(4)
    ];

    [Reactive] 
    public partial List<string> SearchItemsText { get; set; }

    [Reactive] 
    public partial string SelectedDisplay { get; set; }

    [Reactive] 
    public partial int SelectedValue { get; set; }

    [Reactive]
    [ReactiveLinkedProperties(nameof(MessageBoxResultDisplayText))]
    public partial string? MessageBoxResult { get; set; }

    public List<string> LinkedEventDropdown => ["Option 1", "Option 2", "Option 3"];

    [Reactive]
    [ReactiveLinkedEvent(nameof(OnLinkedEventSelection))]
    public partial string LinkedEventSelection { get; set; }

#pragma warning disable CS0067 // Mark members as static
    public event EventHandler? OnLinkedEventSelection; 
#pragma warning restore CS0067 // Mark members as static

    public string MessageBoxResultDisplayText => $"Message Box Result: {MessageBoxResult ?? "N/A"}";
    
    public Func<string, string>? UpdateEnumDescription => s => s + " Updated";

    public Func<Enum?, bool> FilterEnum => a => a != null && (TestEnums)a is TestEnums.ValueOne or TestEnums.ValueTwo;

    [Reactive] 
    public partial bool CheckboxTest { get; set; }

    [Reactive]
    public partial bool? NullableCheckboxTest { get; set; }
    
    [Reactive]
    public partial bool MenuItemCheckboxTest { get; set; }

    public MainWindowViewModel()
    {
        TestString = "Hello World";
        TestPath = string.Empty;
        FolderPath = string.Empty;
        SearchItemsText = [];
        SelectedDisplay = string.Empty;
        LinkedEventSelection = LinkedEventDropdown.Last();
        TestEnum2 = TestEnums.ValueTwo;
    }
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