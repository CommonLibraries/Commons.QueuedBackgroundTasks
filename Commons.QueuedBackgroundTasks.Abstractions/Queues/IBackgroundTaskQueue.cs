namespace Commons.QueuedBackgroundTasks.Abstractions.Queues;

public interface IBackgroundTaskQueue
{
    Task Enqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default);
    Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default);
}

public abstract class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly IList<IBackgroundTaskQueueFilter> filters;

    public DefaultBackgroundTaskQueue(IEnumerable<IBackgroundTaskQueueFilter> filters)
    {
        this.filters = filters.ToList();
    }

    public abstract Task Enqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default);
    public abstract Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default);

    protected async Task BeforeEnqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default)
    {
        var context = new BackgroundTaskQueueFilterContext()
        {
            IsEnqueueAllowed = true,
            CancellationToken = cancellationToken
        };

        foreach (var filter in filters)
        {
            await filter.Execute(backgroundTask, context);
        }
    }
}
