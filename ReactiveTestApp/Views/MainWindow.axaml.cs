using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaControls;
using AvaloniaControls.Controls;
using AvaloniaControls.Extensions;
using AvaloniaControls.Models;
using AvaloniaControls.Services;
using ReactiveTestApp.Services;
using ReactiveTestApp.ViewModels;

namespace ReactiveTestApp.Views;

public partial class MainWindow : RestorableWindow
{
    private MainWindowViewModel _model = new();
    private MainWindowService? _service;
    
    public MainWindow()
    {
        InitializeComponent();
        if (Design.IsDesignMode)
        {
            DataContext = _model;
            return;
        }

        _service = this.GetControlService<MainWindowService>();
        DataContext = _model = _service?.InitializeModel() ?? _model;
        
        _model.OnLinkedEventSelection += async (sender, args) =>
        {
            await MessageWindow.ShowMessageDialog($"You selected {_model.LinkedEventSelection}", "Linked Event Result");
        };
    }

    protected override string RestoreFilePath => "test.json";
    protected override int DefaultWidth => 800;
    protected override int DefaultHeight => 600;

    private void RangeBase_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        GlobalScaleFactor = e.NewValue;
    }

    private void FileControl_OnOnUpdated(object? sender, FileControlUpdatedEventArgs e)
    {
        _service?.Log(e.Path ?? "N/A");
    }

    private void HeaderButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _service?.Log("Header button clicked");
    }

    private void BasicMessageBoxButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.YesNo,
            Message = "Hello! Click Yes to accept or No to decline.",
            Title = "Basic Message Window",
            Icon = MessageWindowIcon.Info
        });
        messageWindow.ShowDialog(control);
        messageWindow.Closed += (o, args) =>
        {
            _model.MessageBoxResult = messageWindow.DialogResult?.PressedAcceptButton == true ? "Accepted" : "Declined";
        };
        _model.MessageBoxResult = "Window opened!";
    }

    private void LargeMessageBoxButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.Close,
            Message = "orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            Title = "Long Message Window"
        });
        messageWindow.Show(this);
    }

    private void CheckboxMessageBoxButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.OK,
            Icon = MessageWindowIcon.Question,
            Message = "Hello there! You can check the box, click on the link, or type in the text box.",
            Title = "Basic Message Window",
            CheckBoxText = "Select this to do something!",
            DisplayTextBox = true,
            LinkText = "Click here to go to Google",
            LinkUrl = "https://www.google.com"
        });
        messageWindow.ShowDialog(control);
        messageWindow.Closed += (o, args) =>
        {
            var accepted = messageWindow.DialogResult?.PressedAcceptButton == true ? "Accepted" : "Declined";
            var didCheck = messageWindow.DialogResult?.CheckedBox == true ? "Checked" : "Not Checked";
            var text = messageWindow.DialogResult?.ResponseText == null
                ? "N/A"
                : messageWindow.DialogResult.ResponseText;
            _model.MessageBoxResult = $"Accepted: {accepted} | Checkbox: {didCheck} | TextBox: {text}";
        };
        _model.MessageBoxResult = "Window opened!";
    }

    private void UIExceptionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void TaskExceptionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ITaskService.Run(() =>
        {
            throw new System.NotImplementedException();
        });
    }

    private void IconButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _service?.Log("Icon button clicked");
    }

    private void TestMappingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _service?.TestMapping();
    }

    private void MenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        _model.MenuItemCheckboxTest = !_model.MenuItemCheckboxTest;
    }

    private void InfiniteLoadingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Message = "Loading Message",
            ProgressBar = MessageWindowProgressBarType.Indeterminate,
            PrimaryButtonText = "Cancel"
        });
        window.ShowDialog(this);
    }

    private void RegularLoadingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Message = "Loading Message",
            ProgressBar = MessageWindowProgressBarType.Normal,
            PrimaryButtonText = "Cancel"
        });
        window.ShowDialog(this);

        Dispatcher.UIThread.Invoke(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            window.UpdateProgressBar(50);
            await Task.Delay(TimeSpan.FromSeconds(2));
            window.UpdateProgressBar(100);
        });
    }

    private void UpdateMessageWindowButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new MessageWindow(new MessageWindowRequest()
        {
            Message = "Loading Message",
            PrimaryButtonText = "Cancel"
        });
        window.ShowDialog(this);

        Dispatcher.UIThread.Invoke(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            window.UpdateMessageText("Update 1");
            await Task.Delay(TimeSpan.FromSeconds(1));
            window.UpdatePrimaryButtonText("Update 2");
            await Task.Delay(TimeSpan.FromSeconds(1));
            window.UpdateSecondaryButtonText("Update 3");
            window.ToggleSecondaryButton(true);
        });
    }
}