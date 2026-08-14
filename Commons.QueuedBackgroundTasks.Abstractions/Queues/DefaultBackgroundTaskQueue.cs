using Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

namespace Commons.QueuedBackgroundTasks.Abstractions.Queues;

public abstract class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly IList<IBackgroundTaskQueueFilter> filters;

    public DefaultBackgroundTaskQueue(IEnumerable<IBackgroundTaskQueueFilter> filters
        )
    {
        this.filters = filters.ToList();
    }

    public async Task Enqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default)
    {
        var context = new EnqueuingContext()
        {
            IsEnqueueAllowed = true,
            CancellationToken = cancellationToken
        };

        var enqueuedContext = new EnqueuedContext()
        {
            CancellationToken = cancellationToken
        };

        foreach (var filter in filters)
        {
            await filter.OnEnqueuing(backgroundTask, context);
        }

        if (!context.IsEnqueueAllowed)
        {
            return;
        }

        await this.Enqueue(backgroundTask, context, enqueuedContext, cancellationToken);

        foreach (var filter in filters)
        {
            await filter.OnEnqueued(backgroundTask, enqueuedContext);
        }
    }

    protected abstract Task Enqueue(
        IBackgroundTask backgroundTask,
        EnqueuingContext enqueuingContext,
        EnqueuedContext enqueuedContext,
        CancellationToken cancellationToken = default);

    public abstract Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default);
}
