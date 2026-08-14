using System.Threading.Channels;
using Commons.QueuedBackgroundTasks.Abstractions;
using Commons.QueuedBackgroundTasks.Abstractions.Queues;
using Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

namespace Commons.QueuedBackgroundTasks.InMemoryQueues;

public class InMemoryBackgroundTaskQueue : DefaultBackgroundTaskQueue
{
    private readonly Channel<IBackgroundTask> tasks = Channel.CreateUnbounded<IBackgroundTask>();
    private readonly IList<IBackgroundTaskQueueFilter> filters;
    public InMemoryBackgroundTaskQueue(IEnumerable<IBackgroundTaskQueueFilter> filters) : base(filters)
    {
        this.filters = filters.ToList();
    }

    protected override async Task Enqueue(
        IBackgroundTask backgroundTask,
        EnqueuingContext enqueuingContext,
        EnqueuedContext enqueuedContext,
        CancellationToken cancellationToken = default)
    {
        if (!tasks.Writer.TryWrite(backgroundTask))
        {
            throw new InvalidOperationException("Failed to enqueue the background task.");
        }
    }

    public override async Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default)
    {
        var backgroundTask = await tasks.Reader.ReadAsync(cancellationToken);
        return backgroundTask;
    }
}
