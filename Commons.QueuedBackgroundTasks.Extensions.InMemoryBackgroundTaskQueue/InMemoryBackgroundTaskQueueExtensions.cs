using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.InMemoryBackgroundTaskQueue;

public static class InMemoryBackgroundTaskQueueExtensions
{
    public static IServiceCollection AddInMemoryBackgroundTaskQueue(IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, InMemoryBackgroundTaskQueue>();
        return services;
    }
}
