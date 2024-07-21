using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using DynamicData;
using System;

namespace AvaloniaControls.Controls;

public partial class BoolComboBox : UserControl
{
    private static readonly bool[] NonNullValues = [true, false];
    private static readonly bool?[] NullValues = [null, true, false];
    private List<string> _options = [];
    
    public BoolComboBox()
    {
        InitializeComponent();

        ValueProperty.Changed.Subscribe(x =>
        {
            if (x.Sender != this) return;
            UpdateComboBox();
        });
    }
    
    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<EnumComboBox, object?>(
        nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<bool> AllowNullsProperty = AvaloniaProperty.Register<EnumComboBox, bool>(
        nameof(AllowNulls));

    public bool AllowNulls
    {
        get => GetValue(AllowNullsProperty);
        set => SetValue(AllowNullsProperty, value);
    }
    
    public static readonly StyledProperty<string> TrueDisplayTextProperty = AvaloniaProperty.Register<EnumComboBox, string>(
        nameof(TrueDisplayText), defaultValue: "Yes");

    public string TrueDisplayText
    {
        get => GetValue(TrueDisplayTextProperty);
        set => SetValue(TrueDisplayTextProperty, value);
    }
    
    public static readonly StyledProperty<string> FalseDisplayTextProperty = AvaloniaProperty.Register<EnumComboBox, string>(
        nameof(FalseDisplayText), defaultValue: "No");

    public string FalseDisplayText
    {
        get => GetValue(FalseDisplayTextProperty);
        set => SetValue(FalseDisplayTextProperty, value);
    }
    
    public static readonly StyledProperty<string> NullDisplayTextProperty = AvaloniaProperty.Register<EnumComboBox, string>(
        nameof(NullDisplayText), defaultValue: " ");

    public string NullDisplayText
    {
        get => GetValue(NullDisplayTextProperty);
        set => SetValue(NullDisplayTextProperty, value);
    }
    
    private void MainComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0) return;
        var selectedText = e.AddedItems[0] as string ?? "";
        Value = AllowNulls ? NullValues[_options.IndexOf(selectedText)] : NonNullValues[_options.IndexOf(selectedText)];
    }

    private void MainComboBox_OnLoaded(object? sender, RoutedEventArgs e)
    {
        UpdateComboBox();
    }

    private void UpdateComboBox()
    {
        string value;
        
        if (AllowNulls)
        {
            _options = [NullDisplayText, TrueDisplayText, FalseDisplayText];
            value = _options[NullValues.IndexOf(Value as bool?)];
        }
        else
        {
            _options = [TrueDisplayText, FalseDisplayText];
            value = _options[NonNullValues.IndexOf(Value as bool? ?? true)];
        }

        var comboBox = this.Find<ComboBox>(nameof(MainComboBox))!;
        comboBox.ItemsSource = _options;
        comboBox.SelectedValue = value;
    }
}