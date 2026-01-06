using System.Reflection;
using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Commons.QueuedBackgroundTasks.Extensions;

public static class Extensions
{
    public static IServiceCollection AddBackgroundTaskHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.TryAddTransient<IBackgroundTaskDispatcher, DefaultBackgroundTaskDispatcher>();

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
