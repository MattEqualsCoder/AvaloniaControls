using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaControls.Models;

namespace AvaloniaControls.Controls;

public partial class EnumComboBox : UserControl
{
    private readonly ICollection<string> _names = new List<string>();
    private readonly Dictionary<string, object?> _nameValues = new();
    private readonly Dictionary<object, string> _valueDescriptions = new();
    private readonly ComboBox _mainComboBox;
    
    public EnumComboBox()
    {
        InitializeComponent();
        _mainComboBox = this.Find<ComboBox>(nameof(MainComboBox))!;
        
        ValueProperty.Changed.Subscribe(x =>
        {
            UpdateSelectedValue();
        });
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
        nameof(Value), defaultBindingMode: BindingMode.TwoWay);

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
    
    public static readonly StyledProperty<Func<Enum?, bool>?> FilterProperty = AvaloniaProperty.Register<EnumComboBox, Func<Enum?, bool>?>(
        nameof(Value));
    
    public Func<Enum?, bool>? Filter
    {
        get => GetValue(FilterProperty);
        set =>SetValue(FilterProperty, value);
    }

    private void PopulateValues(Type? type)
    {
        if (type == null)
        {
            return;
        }
        
        foreach (var enumValue in Enum.GetValues(type))
        {
            if (Filter != null && Filter.Invoke(enumValue as Enum) != true)
            {
                continue;
            }
                
            var description = ((Enum)enumValue).GetDescription();
            if (DescriptionAction != null)
            {
                description = DescriptionAction.Invoke(description);
            }

            if (_names.Contains(description))
            {
                continue;
            }
            
            _names.Add(description);
            _nameValues[description] = enumValue;
            _valueDescriptions[enumValue] = description;
        }

        _mainComboBox.ItemsSource = _names;
        UpdateSelectedValue();
    }

    private void UpdateSelectedValue()
    {
        if (Value != null && _valueDescriptions.TryGetValue(Value, out var value))
        {
            _mainComboBox.SelectedValue = value;    
        }
        else if(_names.Count > 0)
        {
            _mainComboBox.SelectedValue = _names.First();
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