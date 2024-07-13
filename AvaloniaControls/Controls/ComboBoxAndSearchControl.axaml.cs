using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaControls.Models;
using Material.Icons.Avalonia;

namespace AvaloniaControls.Controls;

public partial class ComboBoxAndSearchControl : UserControl
{
    private bool _bound;
    private List<string>? _prevSearchResults;
    private bool _enableSearch;
    private bool _canEnable = true;
    private string? _autoCompleteSelectedItem;
    
    public ComboBoxAndSearchControl()
    {
        InitializeComponent();

        DisplayValueProperty.Changed.Subscribe(x =>
        {
            var item = Items.FirstOrDefault(i => i.Display == x.NewValue.Value);
            if (item == null)
            {
                return;
            }
            this.FindControl<ComboBox>(nameof(ComboBox))!.SelectedValue = item.Display;
            Value = item.Value;
        });
        
        ValueProperty.Changed.Subscribe(x =>
        {
            var item = Items.FirstOrDefault(i => i.Value?.ToString() == x.NewValue.Value?.ToString());
            if (item == null)
            {
                return;
            }
            this.FindControl<ComboBox>(nameof(ComboBox))!.SelectedValue = item.Display;
            DisplayValue = item.Display;
        });
    }
    
    public static readonly StyledProperty<List<ComboBoxAndSearchItem>> ItemsProperty = AvaloniaProperty.Register<ComboBoxAndSearchControl, List<ComboBoxAndSearchItem>>(
        nameof(Items), defaultValue: []);

    public List<ComboBoxAndSearchItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public static readonly StyledProperty<string> DisplayValueProperty = AvaloniaProperty.Register<ComboBoxAndSearchControl, string>(
        nameof(DisplayValue));

    public string DisplayValue
    {
        get => GetValue(DisplayValueProperty);
        set => SetValue(DisplayValueProperty, value);
    }
    
    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<ComboBoxAndSearchControl, object?>(
        nameof(Value));

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public event EventHandler<ComboBoxAndSearchValueChangedArgs>? ValueChanged;

    private void OnValueChanged(ComboBoxAndSearchItem item)
    {
        Value = item.Value;
        DisplayValue = item.Display;
        ValueChanged?.Invoke(this, new ComboBoxAndSearchValueChangedArgs(item));

        var icon = this.FindControl<MaterialIcon>(nameof(DescriptionIcon))!;
        icon.IsVisible = item.HasDescription;

        if (item.HasDescription)
        {
            ToolTip.SetTip(icon, item.Description);   
        }
    }
    
    private void AutoCompleteBox_OnPopulated(object? sender, PopulatedEventArgs e)
    {
        _prevSearchResults = e.Data.Cast<string>().ToList();
        if (_prevSearchResults.Count != 1)
        {
            return;
        }
        DisplayValue = _prevSearchResults.FirstOrDefault() ?? "";
        Search(DisplayValue);
    }

    private void AutoCompleteBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var text = (sender as AutoCompleteBox)?.Text;
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        Search(text);
    }

    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SetEnableSearch(!_enableSearch);
    }

    private void SetEnableSearch(bool newValue)
    {
        if (!_canEnable)
        {
            return;
        }
        
        _enableSearch = newValue;
        
        this.Find<AutoCompleteBox>(nameof(AutoCompleteBox))!.IsVisible = _enableSearch;
        
        if (_enableSearch)
        {
            this.Find<AutoCompleteBox>(nameof(AutoCompleteBox))!.Text = "";
            
            // Delay because setting focus the first time doesn't work for some reason
            Task.Run(() =>
            {
                Thread.Sleep(50);
                Dispatcher.UIThread.Invoke(() =>
                {
                    this.Find<AutoCompleteBox>(nameof(AutoCompleteBox))!.Focus();
                });
            });
        }
        else
        {
            _canEnable = false;
            Task.Run(() =>
            {
                Thread.Sleep(500);
                _canEnable = true;
            });
        }
    }

    private void ComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Search(e.AddedItems.Cast<string>().FirstOrDefault() ?? "");
    }
    
    private void Search(string searchText)
    {
        var selectedItem = Items.FirstOrDefault(x => x.Display == searchText);
        if (selectedItem != null)
        {
            OnValueChanged(selectedItem);   
        }
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (Items.Count == 0)
        {
            return;
        }
        
        var items = Items.Select(x => x.Display).ToList();
        var comboBox = this.Find<ComboBox>(nameof(ComboBox))!;
        var autoComplete = this.Find<AutoCompleteBox>(nameof(AutoCompleteBox))!;
        comboBox.ItemsSource = items;
        autoComplete.ItemsSource = items;

        if (!string.IsNullOrEmpty(DisplayValue))
        {
            var item = Items.FirstOrDefault(x => x.Display == DisplayValue);

            if (item != null)
            {
                OnValueChanged(item);
                comboBox.SelectedValue = item.Display;
                return;
            }
        }

        var selectedItem = Items.FirstOrDefault(x => x.Value?.ToString() == Value?.ToString()) ?? Items.First();
        OnValueChanged(selectedItem);
        comboBox.SelectedValue = selectedItem.Display;
    }

    private void AutoCompleteBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        var element = TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement() as TextBox;

        if (element == null || element.Parent?.Parent != sender)
        {
            SetEnableSearch(false);
        }
    }

    private void AutoCompleteBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (_bound || TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement() is not TextBox element) return;
        element.KeyDown += ElementOnKeyDown;
        _bound = true;
    }

    private void ElementOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key is not (Key.Enter or Key.Return or Key.Tab)) return;
        if (_autoCompleteSelectedItem != null)
        {
            Search(_autoCompleteSelectedItem);
        }
        else if (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text) && _prevSearchResults?.Count > 1 && _prevSearchResults.First().Contains(textBox.Text, StringComparison.OrdinalIgnoreCase))
        {
            Search(_prevSearchResults.First());
        }
        SetEnableSearch(false);
        this.Find<ComboBox>(nameof(ComboBox))!.Focus();
    }

    private void AutoCompleteBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _autoCompleteSelectedItem = e.AddedItems.Cast<string>().FirstOrDefault();
    }
}