using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Commons.QueuedBackgroundTasks.Extensions;

public static class Extensions
{
    public static IBackgroundTaskServiceBuilder AddBackgroundTasks(this IServiceCollection services)
    {
        services.TryAddTransient<IBackgroundTaskDispatcher, DefaultBackgroundTaskDispatcher>();
        return new DefaultBackgroundTaskServiceBuilder(services);
    }
}
