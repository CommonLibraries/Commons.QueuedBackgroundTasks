using System.Reflection;
using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.Extensions;

internal class DefaultBackgroundTaskServiceBuilder : IBackgroundTaskServiceBuilder
{
    private readonly IDictionary<Type, Type>? handlerLookup;
    private readonly IDictionary<Type, string>? contextLookup;
    private readonly IServiceCollection services;

    public DefaultBackgroundTaskServiceBuilder(IServiceCollection services,
        IDictionary<Type, Type>? handlerLookup = null,
        IDictionary<Type, string>? contextLookup = null)
    {
        this.services = services;
        this.handlerLookup = handlerLookup;
        this.contextLookup = contextLookup;
    }

    public IBackgroundTaskServiceBuilder AddBackgroundTaskHandlers(Assembly assembly, string? context = null)
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

                        if (this.handlerLookup is not null)
                        {
                            this.handlerLookup[iface.GenericTypeArguments[0]] = type;
                        }

                        if (this.contextLookup is not null && context is not null)
                        {
                            this.contextLookup[type] = context;
                        }
                    }
                }
            }
        }

        return this;
    }
}
