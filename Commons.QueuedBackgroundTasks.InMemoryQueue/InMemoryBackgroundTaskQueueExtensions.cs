using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.InMemoryQueues;

public static class InMemoryBackgroundTaskQueueExtensions
{
    public static IServiceCollection AddInMemoryBackgroundTaskQueue(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, InMemoryBackgroundTaskQueue>();
        return services;
    }
}
