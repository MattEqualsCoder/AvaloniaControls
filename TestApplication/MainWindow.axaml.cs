using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaControls;
using AvaloniaControls.Controls;
using AvaloniaControls.Models;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TestApplication;

public partial class MainWindow : RestorableWindow
{
    private ILogger<MainWindow>? _logger;
    private TestViewModel? _model;

    public MainWindow() : this(null)
    {
        
    }
    
    public MainWindow(ILogger<MainWindow>? logger)
    {
        _logger = logger;
        InitializeComponent();
        _logger?.LogInformation("Main Window Opened");
        DataContext = _model = new TestViewModel();
    }

    private void FileControl_OnOnUpdated(object? sender, FileControlUpdatedEventArgs e)
    {
        _logger?.LogInformation("Path: {Path}", e.Path);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.OK,
            Message = "Hello there!",
            Title = "Whoa!"
        });
        messageWindow.ShowDialog(control);
        messageWindow.Closed += (o, args) =>
        {
            _logger?.LogInformation(messageWindow.DialogResult?.PressedAcceptButton == true ? "Accepted" : "Declined");
        };
        _logger?.LogInformation("Window opened");
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        MessageWindow.GlobalParentWindow = this;
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.YesNo,
            Icon = MessageWindowIcon.Info,
            Message = "orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            Title = "Whoa!"
        });
        messageWindow.ShowDialog();
    }

    private void Button3_OnClick(object? sender, RoutedEventArgs e)
    {
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.OKCancel,
            Icon = MessageWindowIcon.Error,
            Message = "This is an example of a Window with a checkbox",
            Title = "Whoa!",
            CheckBoxText = "Do you agree?"
        });
        messageWindow.ShowDialog(this);
        messageWindow.Closed += (o, args) =>
        {
            _logger?.LogInformation(messageWindow.DialogResult?.CheckedBox == true ? "Checked" : "Not Checked");
        };
    }

    private void Button4_OnClick(object? sender, RoutedEventArgs e)
    {
        var messageWindow = new MessageWindow(new MessageWindowRequest()
        {
            Buttons = MessageWindowButtons.OKCancel,
            Icon = MessageWindowIcon.Warning,
            Message = "This is an example of a Window with a textbox",
            Title = "Whoa!",
            DisplayTextBox = true
        });
        messageWindow.ShowDialog(this);
        messageWindow.Closed += (o, args) =>
        {
            _logger?.LogInformation(string.IsNullOrWhiteSpace(messageWindow.DialogResult?.ResponseText) ? "N/A" : messageWindow.DialogResult?.ResponseText);
        };
    }

    protected override string RestoreFilePath => "test.json";
    protected override int DefaultWidth { get; } = 800;
    protected override int DefualtHeight { get; } = 600;

    private void EnumComboBox_OnValueChanged(object sender, EnumValueChangedEventArgs args)
    {
        _logger?.LogInformation("New value: {Value}", args.EnumValue);
    }

    private void EnumComboBoxText_OnClick(object? sender, RoutedEventArgs e)
    {
        _logger?.LogInformation("Value: {Value}", _model?.TestEnum);
    }

    private void ButtonConvert_OnClick(object? sender, RoutedEventArgs e)
    {
        GlobalScaleFactor = 1.5;
    }
}