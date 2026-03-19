using System.Reflection;
using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.Extensions;

internal class DefaultBackgroundTaskServiceBuilder : IBackgroundTaskServiceBuilder
{
    private readonly IServiceCollection services;

    public DefaultBackgroundTaskServiceBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public IBackgroundTaskServiceBuilder AddBackgroundTaskHandlers(Assembly assembly)
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

        return this;
    }
}
