using System.Linq;
using AvaloniaControls.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AvaloniaControls.ControlServices;

public static class ControlServiceCollectionExtensions
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
}