namespace Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

public abstract class BackgroundTaskQueueFilter : IBackgroundTaskQueueFilter
{
    public virtual Task OnEnqueued(IBackgroundTask backgroundTask, EnqueuedContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnEnqueuing(IBackgroundTask backgroundTask, EnqueuingContext context)
    {
        return Task.CompletedTask;
    }
}