using Avalonia.Controls;
using AvaloniaControls.Models;

namespace AvaloniaControls.Controls;

public class ExceptionWindow : MessageWindow
{
    public static string GitHubUrl { get; set; } = "";
    public static string LogPath { get; set; } = "";
    
    public static Window? ParentWindow { get; set; }

    public ExceptionWindow()
    {
        var canOpenFolder = !string.IsNullOrWhiteSpace(LogPath);
        var canOpenGitHub = !string.IsNullOrWhiteSpace(GitHubUrl);
        var canPerformAction = canOpenFolder || canOpenGitHub;

        Owner = ParentWindow;

        if (ParentWindow != null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
            
        Closing += (sender, args) =>
        {
            if (DialogResult?.PressedAcceptButton != true || !canPerformAction)
            {
                return;
            }

            if (canOpenFolder)
            {
                CrossPlatformTools.OpenDirectory(LogPath);    
            }
            
            if (DialogResult?.CheckedBox == true && canOpenGitHub)
            {
                CrossPlatformTools.OpenUrl(GitHubUrl);
            }
        };

        InitializeComponent();
        DataContext = new MessageWindowRequest()
        {
            Message = "A critical error occurred. Please open an the issue on GitHub to report the problem." +
                      (canOpenFolder ? "\nPress Yes to open the log directory." : ""),
            Title = "Error",
            Buttons = canPerformAction ? MessageWindowButtons.YesNo : MessageWindowButtons.OK,
            Icon = MessageWindowIcon.Error,
            CheckBoxText = canOpenGitHub ? "Open the GitHub issue page" : ""
        };
    }
}