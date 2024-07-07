using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaControls.Controls;
using AvaloniaControls.ControlServices;
using AvaloniaControls.Extensions;
using AvaloniaControls.Models;
using AvaloniaControls.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveTestApp.Views;
using Serilog;

namespace ReactiveTestApp;

sealed class Program
{
    internal static IHost? MainHost { get; private set; }
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        MainHost = Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureLogging(logging =>
            {
                logging.AddSerilog(dispose: true);
            })
            .ConfigureServices(services =>
            {
                services.AddAvaloniaControlServices<Program>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
        
        MainHost.Services.GetRequiredService<ITaskService>();
        MainHost.Services.GetRequiredService<IControlServiceFactory>();
        
        ExceptionWindow.GitHubUrl = "https://github.com/MattEqualsCoder";
        ExceptionWindow.LogPath = Directory.GetCurrentDirectory();
        
        using var source = new CancellationTokenSource();
        
        try
        {
           BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            ShowExceptionPopup(e).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
            Dispatcher.UIThread.MainLoop(source.Token);
        } 
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    
    private static async Task ShowExceptionPopup(Exception e)
    {
        Log.Error(e, "[CRASH] Uncaught {Name}: ", e.GetType().Name);
        var window = new ExceptionWindow();
        if (ExceptionWindow.ParentWindow != null)
        {
            await window.ShowDialog(ExceptionWindow.ParentWindow);
        }
        else
        {
            window.Show();
        }
        
        await Dispatcher.UIThread.Invoke(async () =>
        {
            while (window.IsVisible)
            {
                await Task.Delay(500);
            }
        });
    }
}