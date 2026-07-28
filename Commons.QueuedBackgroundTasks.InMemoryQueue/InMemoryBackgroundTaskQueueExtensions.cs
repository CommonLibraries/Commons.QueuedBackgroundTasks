using Commons.QueuedBackgroundTasks.Abstractions.Queues;
using Commons.QueuedBackgroundTasks.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Commons.QueuedBackgroundTasks.InMemoryQueues;

public static class InMemoryBackgroundTaskQueueExtensions
{
    public static IBackgroundTaskServiceBuilder UseInMemoryBackgroundTaskQueue(this IBackgroundTaskServiceBuilder serviceBuilder)
    {
        serviceBuilder.UseQueue<InMemoryBackgroundTaskQueue>();
        return serviceBuilder;
    }
}
