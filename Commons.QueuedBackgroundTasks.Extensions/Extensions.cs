using System.Reflection;
using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.Extensions;

public static class Extensions
{
    public static IServiceCollection AddBackgroundTaskService(this IServiceCollection services)
    {
        services.AddTransient<IBackgroundTaskDispatcher, DefaultBackgroundTaskDispatcher>();
        return services;    
    }

    public static IServiceCollection AddBackgroundTaskHandlers(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsClass && !type.IsAbstract)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IBackgroundTaskHandler<>))
                    {
                        services.AddTransient(iface, type);
                    }
                }
            }
        }

        return services;
    }
}
