using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaControls.Models;

namespace AvaloniaControls.Controls;

public partial class MessageWindow : ScalableWindow
{
    public static Window? GlobalParentWindow { get; set; }
    
    public MessageWindow()
    {
        InitializeComponent();
        DataContext = new MessageWindowRequest()
        {
            Message = "Unknown",
            LinkText = "Hi",
            LinkUrl = "Hello",
            CheckBoxText = "Testing!",
            Icon = MessageWindowIcon.Info
        };
    }

    public MessageWindow(MessageWindowRequest request)
    {
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        InitializeComponent();
        DataContext = request;
    }
    
    public MessageWindowResult? DialogResult { get; set; }

    public void ShowDialog()
    {
        if (GlobalParentWindow == null)
        {
            throw new InvalidOperationException("Cannot show dialog without a parent window");
        }
        ShowDialog(GlobalParentWindow);
    }
    
    public void ShowDialog(Control control)
    {
        while (control is not Window)
        {
            if (control.Parent is Control parentControl)
            {
                control = parentControl;
            }
            else
            {
                break;
            }
        }

        var window = control as Window ?? GlobalParentWindow ?? throw new InvalidOperationException("Cannot show dialog without a parent window");
        Icon = window.Icon;
        Owner = window;
        base.ShowDialog(window);
    }

    private void Button1_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogResult = new MessageWindowResult()
        {
            PressedAcceptButton = true,
            CheckedBox = this.FindControl<CheckBox>(nameof(ResponseCheckBox))?.IsChecked == true,
            ResponseText = this.FindControl<TextBox>(nameof(ResponseTextBox))?.Text
        };
        Close(DialogResult);
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogResult = new MessageWindowResult()
        {
            PressedAcceptButton = false,
            CheckedBox = this.FindControl<CheckBox>(nameof(ResponseCheckBox))?.IsChecked == true,
            ResponseText = this.FindControl<TextBox>(nameof(ResponseTextBox))?.Text
        };
        Close(DialogResult);
    }

    private void ResponseLink_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MessageWindowRequest request || string.IsNullOrEmpty(request.LinkUrl))
        {
            return;
        }

        if (request.LinkUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            CrossPlatformTools.OpenUrl(request.LinkUrl);
        }
        else
        {
            CrossPlatformTools.OpenDirectory(request.LinkUrl);
        }
    }
}