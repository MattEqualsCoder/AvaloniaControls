using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Material.Icons;
using System;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AvaloniaControls.Controls;

public partial class IconCheckbox : UserControl
{
    public IconCheckbox()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode)
        {
            Text = "Checkbox Text";
        }

        if (!AllowNulls && Value is null)
        {
            Value = false;
        }
        
        ValueProperty.Changed.Subscribe(x =>
        {
            Update();
        });
        
        Update();
    }
    
    public static readonly StyledProperty<MaterialIconKind?> PrefixIconKindProperty = AvaloniaProperty.Register<IconCheckbox, MaterialIconKind?>(
        nameof(PrefixIconKind));

    public MaterialIconKind? PrefixIconKind
    {
        get => GetValue(PrefixIconKindProperty);
        set => SetValue(PrefixIconKindProperty, value);
    }
    
    public static readonly StyledProperty<MaterialIconKind> CheckedIconKindProperty = AvaloniaProperty.Register<IconCheckbox, MaterialIconKind>(
        nameof(CheckedIconKind), MaterialIconKind.CheckboxOutline);

    public MaterialIconKind CheckedIconKind
    {
        get => GetValue(CheckedIconKindProperty);
        set => SetValue(CheckedIconKindProperty, value);
    }
    
    public static readonly StyledProperty<MaterialIconKind> UncheckedIconKindProperty = AvaloniaProperty.Register<IconCheckbox, MaterialIconKind>(
        nameof(UncheckedIconKind), MaterialIconKind.CheckboxBlankOutline);

    public MaterialIconKind UncheckedIconKind
    {
        get => GetValue(UncheckedIconKindProperty);
        set => SetValue(UncheckedIconKindProperty, value);
    }
    
    public static readonly StyledProperty<MaterialIconKind> NullIconKindProperty = AvaloniaProperty.Register<IconCheckbox, MaterialIconKind>(
        nameof(NullIconKind), MaterialIconKind.QuestionBoxOutline);

    public MaterialIconKind NullIconKind
    {
        get => GetValue(NullIconKindProperty);
        set => SetValue(NullIconKindProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> CheckedBrushProperty = AvaloniaProperty.Register<IconCheckbox, IBrush?>(
        nameof(CheckedBrush));

    public IBrush? CheckedBrush
    {
        get => GetValue(CheckedBrushProperty);
        set => SetValue(CheckedBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> UncheckedBrushProperty = AvaloniaProperty.Register<IconCheckbox, IBrush?>(
        nameof(UncheckedBrush));

    public IBrush? UncheckedBrush
    {
        get => GetValue(UncheckedBrushProperty);
        set => SetValue(UncheckedBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> NullBrushProperty = AvaloniaProperty.Register<IconCheckbox, IBrush?>(
        nameof(NullBrush));

    public IBrush? NullBrush
    {
        get => GetValue(NullBrushProperty);
        set => SetValue(NullBrushProperty, value);
    }
    
    public bool HasPrefixIcon => PrefixIconKind.HasValue;
    
    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<IconCheckbox, object?>(
        nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<bool> AllowNullsProperty = AvaloniaProperty.Register<IconCheckbox, bool>(
        nameof(AllowNulls));

    public bool AllowNulls
    {
        get => GetValue(AllowNullsProperty);
        set => SetValue(AllowNullsProperty, value);
    }
    
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<IconCheckbox, string>(
        nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<string> CheckedTextProperty = AvaloniaProperty.Register<IconCheckbox, string>(
        nameof(CheckedText));

    public string CheckedText
    {
        get => GetValue(CheckedTextProperty);
        set => SetValue(CheckedTextProperty, value);
    }
    
    public static readonly StyledProperty<string> UncheckedTextProperty = AvaloniaProperty.Register<IconCheckbox, string>(
        nameof(UncheckedText));

    public string UncheckedText
    {
        get => GetValue(UncheckedTextProperty);
        set => SetValue(UncheckedTextProperty, value);
    }
    
    public static readonly StyledProperty<string> NullTextProperty = AvaloniaProperty.Register<IconCheckbox, string>(
        nameof(NullText));

    public string NullText
    {
        get => GetValue(NullTextProperty);
        set => SetValue(NullTextProperty, value);
    }
    
    public static readonly StyledProperty<int> IconSizeProperty = AvaloniaProperty.Register<IconCheckbox, int>(
        nameof(IconSize), 20);

    public int IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }
    
    public event EventHandler<OnIconCheckboxCheckedEventArgs>? OnChecked;

    public void Update()
    {
        if (AllowNulls)
        {
            if (Value is null)
            {
                MainIcon.Kind = NullIconKind;
                if (NullBrush != null)
                {
                    MainIcon.Foreground = NullBrush;
                }
                MainTextBlock.Text = string.IsNullOrEmpty(NullText) ? Text : NullText;
            }
            else if (Value is true)
            {
                MainIcon.Kind = CheckedIconKind;
                if (CheckedBrush != null)
                {
                    MainIcon.Foreground = CheckedBrush;
                }
                MainTextBlock.Text = string.IsNullOrEmpty(CheckedText) ? Text : CheckedText;
            }
            else
            {
                MainIcon.Kind = UncheckedIconKind;
                if (UncheckedBrush != null)
                {
                    MainIcon.Foreground = UncheckedBrush;
                }
                MainTextBlock.Text = string.IsNullOrEmpty(UncheckedText) ? Text : UncheckedText;
            }
        }
        else
        {
            if (Value is true)
            {
                MainIcon.Kind = CheckedIconKind;
                if (CheckedBrush != null)
                {
                    MainIcon.Foreground = CheckedBrush;
                }
                MainTextBlock.Text = string.IsNullOrEmpty(CheckedText) ? Text : CheckedText;
            }
            else
            {
                MainIcon.Kind = UncheckedIconKind;
                if (UncheckedBrush != null)
                {
                    MainIcon.Foreground = UncheckedBrush;
                }
                MainTextBlock.Text = string.IsNullOrEmpty(UncheckedText) ? Text : UncheckedText;
            }
        }

        PrefixIcon.Width = IconSize;
        PrefixIcon.Height = IconSize;
        MainIcon.Width = IconSize;
        MainIcon.Height = IconSize;
        PrefixIcon.IsVisible = PrefixIconKind.HasValue;
        MainTextBlock.IsVisible = !string.IsNullOrEmpty(MainTextBlock.Text);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        bool? val = false;
        if (AllowNulls)
        {
            if (Value is null)
            {
                Value = val = false;
            }
            else if (Value is true)
            {
                Value = val = null;
            }
            else
            {
                Value = val = true;
            }
        }
        else
        {
            if (Value is true)
            {
                Value = val = false;
            }
            else
            {
                Value = val = true;
            }
        }
        
        OnChecked?.Invoke(this, new OnIconCheckboxCheckedEventArgs(val));
    }
}