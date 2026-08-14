namespace Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

public interface IBackgroundTaskQueueFilter
{
    Task OnEnqueuing(IBackgroundTask backgroundTask, EnqueuingContext context);
    Task OnEnqueued(IBackgroundTask backgroundTask, EnqueuedContext context);
}
