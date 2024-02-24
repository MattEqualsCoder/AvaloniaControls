using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using File = System.IO.File;

namespace AvaloniaControls.Controls;

public partial class FileControl : UserControl
{
    public FileControl()
    {
        InitializeComponent();

        if (FileInputType == FileInputControlType.OpenFile)
        {
            AddHandler(DragDrop.DropEvent, DropFile);    
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public static readonly StyledProperty<string?> FilePathProperty = AvaloniaProperty.Register<FileControl, string?>(
        "FilePath");

    public string? FilePath
    {
        get => GetValue(FilePathProperty);
        set => SetValue(FilePathProperty, value);
    }

    public static readonly StyledProperty<string> ButtonTextProperty = AvaloniaProperty.Register<FileControl, string>(
        "ButtonText", "Browse...");

    public string ButtonText
    {
        get => GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public static readonly StyledProperty<bool> ShowClearButtonProperty = AvaloniaProperty.Register<FileControl, bool>(
        "ShowClearButton", true);

    public bool ShowClearButton
    {
        get => GetValue(ShowClearButtonProperty);
        set => SetValue(ShowClearButtonProperty, value);
    }

    public static readonly StyledProperty<FileInputControlType> FileInputTypeProperty = AvaloniaProperty.Register<FileControl, FileInputControlType>(
        "FileInputType");

    public FileInputControlType FileInputType
    {
        get => GetValue(FileInputTypeProperty);
        set => SetValue(FileInputTypeProperty, value);
    }

    public static readonly StyledProperty<string> FilterProperty = AvaloniaProperty.Register<FileControl, string>(
        "Filter", "All Files:*");

    public string Filter
    {
        get => GetValue(FilterProperty);
        set => SetValue(FilterProperty, value);
    }

    public static readonly StyledProperty<string?> DialogTitleProperty = AvaloniaProperty.Register<FileControl, string?>(
        "DialogTitle");

    public string? DialogTitle
    {
        get => GetValue(DialogTitleProperty);
        set => SetValue(DialogTitleProperty, value);
    }
    
    public static readonly StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<FileControl, string>(
        "Watermark", "");

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
    
    public static readonly StyledProperty<string> DefaultPathProperty = AvaloniaProperty.Register<FileControl, string>(
        "DefaultPath", "");

    public string DefaultPath
    {
        get => GetValue(DefaultPathProperty);
        set => SetValue(DefaultPathProperty, value);
    }

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        FilePath = "";
        OnUpdated?.Invoke(this, new FileControlUpdatedEventArgs(FilePath!));
    }

    public static readonly StyledProperty<bool> WarnOnOverwriteProperty = AvaloniaProperty.Register<FileControl, bool>(
        "WarnOnOverwrite", true);

    public bool WarnOnOverwrite
    {
        get => GetValue(WarnOnOverwriteProperty);
        set => SetValue(WarnOnOverwriteProperty, value);
    }
    
    public static readonly StyledProperty<string?> ForceExtensionProperty = AvaloniaProperty.Register<FileControl, string?>(
        "ForceExtension", null);

    public string? ForceExtension
    {
        get => GetValue(ForceExtensionProperty);
        set => SetValue(ForceExtensionProperty, value);
    }

    public event EventHandler<FileControlUpdatedEventArgs>? OnUpdated;

    private static string? PreviousFolder;
    
    private void DropFile(object? sender, DragEventArgs e)
    {
        var file = e.Data?.GetFiles()?.FirstOrDefault();
        if (file == null)
        {
            return;
        }

        var path = file.Path.LocalPath;
        var attr = File.GetAttributes(path);
        bool isDirectory = (attr & FileAttributes.Directory) == FileAttributes.Directory;
        
        if (FileInputType == FileInputControlType.OpenFile && !isDirectory && VerifyFileMeetsFilter(path))
        {
            FilePath = path;
            OnUpdated?.Invoke(this, new FileControlUpdatedEventArgs(FilePath!));    
        }
        else if (FileInputType == FileInputControlType.SaveFile && !isDirectory && VerifyFileMeetsFilter(path))
        {
            FilePath = path;
            OnUpdated?.Invoke(this, new FileControlUpdatedEventArgs(FilePath!));    
        }
        else if (FileInputType == FileInputControlType.Folder && isDirectory)
        {
            FilePath = path;
            OnUpdated?.Invoke(this, new FileControlUpdatedEventArgs(FilePath!)); 
        }
    }

    private async void BrowseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is not Window _window)
        {
            throw new InvalidOperationException();
        }

        var locationPath = "";

        if (!string.IsNullOrEmpty(FilePath) && (File.Exists(FilePath) || Directory.Exists(FilePath)))
        {
            var attr = File.GetAttributes(FilePath);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                locationPath = FilePath;
            }
            else
            {
                var file = new FileInfo(FilePath);
                locationPath = file.DirectoryName;
            }
        }

        if (string.IsNullOrEmpty(locationPath))
        {
            if (!string.IsNullOrEmpty(DefaultPath))
            {
                locationPath = DefaultPath;
            }
            else if (!string.IsNullOrEmpty(PreviousFolder))
            {
                locationPath = PreviousFolder;
            }
            else
            {
                locationPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        var selectedPath = await CrossPlatformTools.OpenFileDialogAsync(_window, FileInputType, Filter, locationPath, DialogTitle,
            WarnOnOverwrite);

        if (selectedPath == null)
        {
            return;
        }

        if (selectedPath is IStorageFile selectedFile)
        {
            PreviousFolder = (await selectedFile.GetParentAsync())?.TryGetLocalPath();
            FilePath = selectedFile.TryGetLocalPath();
        }
        else if (selectedPath is IStorageFolder selectedFolder)
        {
            PreviousFolder = selectedFolder.TryGetLocalPath();
            FilePath = PreviousFolder;
        }
        
        OnUpdated?.Invoke(this, new FileControlUpdatedEventArgs(FilePath!));
    }

    private bool VerifyFileMeetsFilter(string file)
    {
        if (FileInputType == FileInputControlType.Folder)
        {
            return true;
        }
        
        if (Filter == "All Files:*.*")
        {
            return true;
        }
        
        try
        {
            var regexParts = Filter.Split(";").Select(x => x.Split(":")[1].Replace(".", "\\.").Replace("*", ".*"));
            var regex = $"({string.Join("|", regexParts)})";
            return Regex.IsMatch(file, regex);
        }
        catch
        {
            // Just ignore it
        }

        return false;
    }
}