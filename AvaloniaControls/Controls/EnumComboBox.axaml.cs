using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaControls.Models;

namespace AvaloniaControls.Controls;

public partial class EnumComboBox : UserControl
{
    private ICollection<string> _names = new List<string>();
    private Dictionary<string, object?> _nameValues = new();
    private Dictionary<object, string> _valueDescriptions = new();
    private ComboBox _mainComboBox;
    
    public EnumComboBox()
    {
        InitializeComponent();
        _mainComboBox = this.Find<ComboBox>(nameof(MainComboBox))!;
    }

    public event EnumValueChangedEventHandler? ValueChanged;
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public static readonly StyledProperty<Type?> EnumTypeProperty = AvaloniaProperty.Register<EnumComboBox, Type?>(
        nameof(EnumType));

    public Type? EnumType
    {
        get => GetValue(EnumTypeProperty);
        set
        {
            PopulateValues(value);
            SetValue(EnumTypeProperty, value);   
        }
    }
    
    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<EnumComboBox, object?>(
        nameof(Value));

    public object? Value
    {
        get => GetValue(ValueProperty);
        set
        {
            if (value == null)
            {
                throw new InvalidOperationException("Value must not be null");
            }
            SetValue(ValueProperty, value);
            _mainComboBox.SelectedValue = _valueDescriptions[value];
        }
    }
    
    public static readonly StyledProperty<Func<string, string>?> DescriptionActionProperty = AvaloniaProperty.Register<EnumComboBox, Func<string, string>?>(
        nameof(Value));

    public Func<string, string>? DescriptionAction
    {
        get => GetValue(DescriptionActionProperty);
        set
        {
            SetValue(DescriptionActionProperty, value);
            if (value == null)
            {
                PopulateValues(EnumType);
            }
        }
    }

    private void PopulateValues(Type? type)
    {
        if (type == null)
        {
            return;
        }
        
        foreach (var enumValue in Enum.GetValues(type))
        {
            var description = ((Enum)enumValue).GetDescription();
            if (DescriptionAction != null)
            {
                description = DescriptionAction.Invoke(description);
            }
            _names.Add(description);
            _nameValues[description] = enumValue;
            _valueDescriptions[enumValue] = description;
        }

        _mainComboBox.ItemsSource = _names;

        if (Value != null)
        {
            _mainComboBox.SelectedValue = _valueDescriptions[Value];    
        }
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        PopulateValues(EnumType);
    }

    private void MainComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems[0] is string item && _nameValues.TryGetValue(item, out var value) && value != null && value != Value)
        {
            Value = value;
            ValueChanged?.Invoke(this, new EnumValueChangedEventArgs((Enum)value));
        }
    }
}