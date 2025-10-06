using System;
using System.Threading.Tasks;
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
    
    public Task ShowDialog(Control control)
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
        return base.ShowDialog(window);
    }

    public static Task ShowInfoDialog(string message, string? title = null, Window? parentWindow = null)
    {
        parentWindow ??= GlobalParentWindow;
        title ??= parentWindow?.Title ?? "Info";
        
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Title = title,
            Message = message,
            Icon = MessageWindowIcon.Info,
            Buttons = MessageWindowButtons.OK
        });
        
        if (parentWindow != null)
        {
            return window.ShowDialog(parentWindow);
        }
        else
        {
            window.ShowDialog();
            return Task.CompletedTask;
        } 
    }
    
    public static Task ShowErrorDialog(string message, string? title = null, Window? parentWindow = null)
    {
        parentWindow ??= GlobalParentWindow;
        title ??= parentWindow?.Title ?? "Error";
        
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Title = title,
            Message = message,
            Icon = MessageWindowIcon.Error,
            Buttons = MessageWindowButtons.OK
        });
        
        if (parentWindow != null)
        {
            return window.ShowDialog(parentWindow);
        }
        else
        {
            window.ShowDialog();
            return Task.CompletedTask;
        } 
    }
    
    public static async Task<bool> ShowYesNoDialog(string message, string? title = null, Window? parentWindow = null)
    {
        parentWindow ??= GlobalParentWindow;
        title ??= parentWindow?.Title ?? "Question";
        
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Title = title,
            Message = message,
            Icon = MessageWindowIcon.Question,
            Buttons = MessageWindowButtons.YesNo
        });
        
        if (parentWindow != null)
        {
            await window.ShowDialog(parentWindow);
        }
        else
        {
            window.ShowDialog();
        } 
        
        return window.DialogResult?.PressedAcceptButton == true;
    }
    
    public static Task ShowMessageDialog(string message, string? title = null, Window? parentWindow = null)
    {
        parentWindow ??= GlobalParentWindow;
        title ??= parentWindow?.Title ?? "Message";
        
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Title = title,
            Message = message,
            Icon = MessageWindowIcon.None,
            Buttons = MessageWindowButtons.OK
        });
        
        if (parentWindow != null)
        {
            return window.ShowDialog(parentWindow);
        }
        else
        {
            window.ShowDialog();
            return Task.CompletedTask;
        } 
    }
    
    public static async Task<MessageWindowResult?> ShowMessageDialog(MessageWindowRequest request, Window? parentWindow = null)
    {
        parentWindow ??= GlobalParentWindow;
        
        var window = new MessageWindow(request);
        
        if (parentWindow != null)
        {
            await window.ShowDialog(parentWindow);
        }
        else
        {
            window.ShowDialog();
        }

        return window.DialogResult;
    }

    public void UpdateProgressBar(double newValue)
    {
        LoadingBar.Value = newValue;
    }

    public void UpdatePrimaryButtonText(string newValue)
    {
        Button1.Content = newValue;
    }
    
    public void UpdateSecondaryButtonText(string newValue)
    {
        Button2.Content = newValue;
    }
    
    public void UpdateTertiaryButtonText(string newValue)
    {
        Button3.Content = newValue;
    }

    public void UpdateMessageText(string newValue)
    {
        MessageTextBlock.Text = newValue;
    }

    public void ToggleSecondaryButton(bool isVisible)
    {
        Button2.IsVisible = isVisible;
    }
    
    public void ToggleTertiaryButton(bool isVisible)
    {
        Button3.IsVisible = isVisible;
    }

    private void Button1_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogResult = new MessageWindowResult()
        {
            PressedButton = ButtonType.Primary,
            CheckedBox = this.FindControl<CheckBox>(nameof(ResponseCheckBox))?.IsChecked == true,
            ResponseText = this.FindControl<TextBox>(nameof(ResponseTextBox))?.Text
        };
        Close(DialogResult);
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogResult = new MessageWindowResult()
        {
            PressedButton = ButtonType.Secondary,
            CheckedBox = this.FindControl<CheckBox>(nameof(ResponseCheckBox))?.IsChecked == true,
            ResponseText = this.FindControl<TextBox>(nameof(ResponseTextBox))?.Text
        };
        Close(DialogResult);
    }
    
    private void Button3_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogResult = new MessageWindowResult()
        {
            PressedButton = ButtonType.Tertiary,
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