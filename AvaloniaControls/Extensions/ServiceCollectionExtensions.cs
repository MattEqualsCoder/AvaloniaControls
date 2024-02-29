using System.Linq;
using AvaloniaControls.ControlServices;
using AvaloniaControls.Models;
using AvaloniaControls.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AvaloniaControls.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControlServices<TAssembly>(this IServiceCollection services)
    {
        var controlServices = typeof(TAssembly).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IControlService)));
        
        foreach (var service in controlServices)
        {
            services.TryAddTransient(service);
        }

        services.AddSingleton<ControlServiceFactory>();
        return services;
    }
    
    public static IServiceCollection AddAvaloniaControlServices<TAssembly>(this IServiceCollection services)
    {
        return services.AddControlServices<TAssembly>()
            .AddSingleton<ITaskService, TaskService>()
            .AddSingleton<IControlServiceFactory, ControlServiceFactory>()
            .AddAutoMapper(x => x.AddProfile(new ViewModelMapperConfig<TAssembly>()));
    }
}