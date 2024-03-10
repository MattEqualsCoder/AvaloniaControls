using System.Linq;
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
            .Where(t => t.BaseType?.Name.StartsWith("ControlService") == true);
        
        foreach (var service in controlServices)
        {
            services.TryAddTransient(service);
            IControlServiceFactory.ControlServiceDictionary[service.Name] = service;
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