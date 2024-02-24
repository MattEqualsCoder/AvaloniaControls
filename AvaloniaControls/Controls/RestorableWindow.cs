using System;
using System.IO;
using System.Net;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using AvaloniaControls.Models;

namespace AvaloniaControls.Controls;

public abstract class RestorableWindow : ScalableWindow
{
    protected WindowRestoreDetails? RestoreDetails { get; set; }
    
    protected abstract string RestoreFilePath { get; }
    protected abstract int DefaultWidth { get; }
    protected abstract int DefualtHeight { get; }

    public RestorableWindow() : base()
    {
        Restore();
        
        if (Design.IsDesignMode)
        {
            return;
        }
        
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RestoreFilePath))
        {
            return;
        }
        
        RestoreDetails = new WindowRestoreDetails()
        {
            X = Position.X,
            Y = Position.Y,
            Width = Width,
            Height = Height,
            IsMaximized = WindowState == WindowState.Maximized
        };

        var contents = JsonSerializer.Serialize(RestoreDetails);
        File.WriteAllText(RestoreFilePath, contents);
    }

    private void Restore()
    {
        if (string.IsNullOrWhiteSpace(RestoreFilePath) || !File.Exists(RestoreFilePath))
        {
            Width = DefaultWidth;
            Height = DefualtHeight;
            return;
        }

        var contents = File.ReadAllText(RestoreFilePath);
        RestoreDetails = JsonSerializer.Deserialize<WindowRestoreDetails>(contents);

        if (RestoreDetails == null)
        {
            return;
        }

        var screen = Screens.ScreenFromPoint(RestoreDetails.GetPosition() +
                                new PixelPoint((int)RestoreDetails.Width / 2, (int)RestoreDetails.Height / 2));
        if (screen == null)
        {
            Width = DefaultWidth;
            Height = DefualtHeight;
            return;
        }
        
        Position = RestoreDetails.GetPosition();
        Width = RestoreDetails.Width;
        Height = RestoreDetails.Height;
        WindowStartupLocation = WindowStartupLocation.Manual;
        WindowState = RestoreDetails.IsMaximized ? WindowState.Maximized : WindowState.Normal;
    }
}