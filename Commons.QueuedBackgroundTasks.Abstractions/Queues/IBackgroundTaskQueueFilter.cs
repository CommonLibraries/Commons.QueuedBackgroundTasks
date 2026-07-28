namespace Commons.QueuedBackgroundTasks.Abstractions.Queues;

public interface IBackgroundTaskQueueFilter
{
    Task Execute (IBackgroundTask backgroundTask, BackgroundTaskQueueFilterContext context);
}
