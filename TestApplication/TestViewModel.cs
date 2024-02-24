using System;
using System.Collections.Generic;
using AvaloniaControls.Converters;
using AvaloniaControls.Models;

namespace TestApplication;

[MapsTo(typeof(TestClass))]
public class TestViewModel : BaseViewModel
{
    public ICollection<EnumDescription> TestEnumDescriptions => EnumDescriptionConverter.GetEnumDescriptions<TestEnums>();
    
    private bool _boolean;
    public bool Boolean
    {
        get => _boolean;
        set => SetField(ref _boolean, value);
    }
    
    private bool? _nullableBoolean;
    public bool? NullableBoolean
    {
        get => _nullableBoolean;
        set => SetField(ref _nullableBoolean, value);
    }

    private TestEnums _testEnum = TestEnums.Option2;

    public TestEnums TestEnum
    {
        get => _testEnum;
        set => SetField(ref _testEnum, value);
    }

    public Func<string, string>? TestFunction => s => s + "a"; 

    private string _testPath = "/home/matt/Music/MSU/Fate";

    public string TestPath
    {
        get => _testPath;
        set => SetField(ref _testPath, value);
    }
    
    private string _testFolderPath = "";

    public string TestFolderPath
    {
        get => _testFolderPath;
        set => SetField(ref _testFolderPath, value);
    }
    
    private double _doubleValue = 1;

    public double DoubleValue
    {
        get => _doubleValue;
        set => SetField(ref _doubleValue, value);
    }

    public List<string> _listBoxItems = ["Item1", "Item2"];
    
    public List<string> ListBoxItems
    {
        get => _listBoxItems;
        set => SetField(ref _listBoxItems, value);
    }

    public ChildViewModel ChildObject { get; set; } = new();

    public List<ChildViewModel> ChildObjects { get; set; } = new();
}