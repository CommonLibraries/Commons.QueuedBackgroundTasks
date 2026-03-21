using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Commons.QueuedBackgroundTasks.Extensions;

public static class QueuedBackgroundTaskExtensions
{
    public static IBackgroundTaskServiceBuilder AddBackgroundTasks(this IServiceCollection services)
    {
        var handlerLookup = new Dictionary<Type, Type>();
        var contextLookup = new Dictionary<Type, string>();
        services.TryAddTransient<IBackgroundTaskDispatcher, DefaultBackgroundTaskDispatcher>();
        services.TryAddTransient<IBackgroundTaskHandlerLookup>(sp =>
            new DefaultBackgroundTaskHandlerLookup(handlerLookup));
        services.TryAddTransient<IBackgroundTaskContextLookup>(sp =>
            new DefaultBackgroundTaskContextLookup(contextLookup));
        return new DefaultBackgroundTaskServiceBuilder(
            services,
            handlerLookup,
            contextLookup);
    }
}
