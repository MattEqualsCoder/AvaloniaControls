using Avalonia;
using System;
using System.Collections.Generic;
using AvaloniaControls.ControlServices;
using AvaloniaControls.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TestApplication;

class Program
{
    public static IHost MainHost { get; private set; } = null!;
    
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
                services.AddAutoMapper(x => x.AddProfile(new ViewModelMapperConfig<Program>()));
                services.AddSingleton<MainWindow>();
                services.AddControlServices<Program>();
            })
            .Build();

        MainHost.Services.GetRequiredService<ControlServiceFactory>();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions() { RenderingMode = new List<Win32RenderingMode>() { Win32RenderingMode.Software }  })
            .With(new X11PlatformOptions() { UseDBusFilePicker = false })
            .WithInterFont()
            .LogToTrace();
}