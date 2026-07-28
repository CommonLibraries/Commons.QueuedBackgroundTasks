using System.Reflection;
using Commons.QueuedBackgroundTasks.Abstractions;
using Commons.QueuedBackgroundTasks.Abstractions.Middlewares;
using Commons.QueuedBackgroundTasks.Abstractions.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Commons.QueuedBackgroundTasks.Extensions;

internal class DefaultBackgroundTaskServiceBuilder : IBackgroundTaskServiceBuilder
{
    private readonly IDictionary<Type, Type>? handlerLookup;
    private readonly IDictionary<Type, string>? contextLookup;
    private readonly IServiceCollection services;

    public IServiceCollection Services => this.services;

    public DefaultBackgroundTaskServiceBuilder(IServiceCollection services,
        IDictionary<Type, Type>? handlerLookup = null,
        IDictionary<Type, string>? contextLookup = null)
    {
        this.services = services;
        this.handlerLookup = handlerLookup;
        this.contextLookup = contextLookup;
    }

    public IBackgroundTaskServiceBuilder UseHostedService<THostedService>() where THostedService : class, IHostedService
    {
        this.services.AddHostedService<THostedService>();
        return this;
    }

    public IBackgroundTaskServiceBuilder UseQueue<TQueue>() where TQueue : class, IBackgroundTaskQueue
    {
        this.services.AddSingleton<IBackgroundTaskQueue, TQueue>();
        return this;
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

    public IBackgroundTaskServiceBuilder UseFilter<TFilter>() where TFilter : class, IBackgroundTaskQueueFilter
    {
        this.services.AddTransient<IBackgroundTaskQueueFilter, TFilter>();
        return this;
    }

    public IBackgroundTaskServiceBuilder UseMiddleware<TMiddleware>() where TMiddleware : class, IBackgroundTaskMiddleware
    {
        this.services.AddTransient<IBackgroundTaskMiddleware, TMiddleware>();
        return this;
    }
}
