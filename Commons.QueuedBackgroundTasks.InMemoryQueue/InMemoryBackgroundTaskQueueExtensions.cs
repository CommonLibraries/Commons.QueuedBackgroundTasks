using Commons.QueuedBackgroundTasks.Extensions;

namespace Commons.QueuedBackgroundTasks.InMemoryQueues;

public static class InMemoryBackgroundTaskQueueExtensions
{
    public static IBackgroundTaskServiceBuilder UseInMemoryBackgroundTaskQueue(this IBackgroundTaskServiceBuilder serviceBuilder)
    {
        serviceBuilder.UseQueue<InMemoryBackgroundTaskQueue>();
        return serviceBuilder;
    }
}
