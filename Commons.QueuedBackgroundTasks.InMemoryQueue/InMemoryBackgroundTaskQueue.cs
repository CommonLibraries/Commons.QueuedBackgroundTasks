using System.Threading.Channels;
using Commons.QueuedBackgroundTasks.Abstractions;
using Commons.QueuedBackgroundTasks.Abstractions.Queues;

namespace Commons.QueuedBackgroundTasks.InMemoryQueues;

public class InMemoryBackgroundTaskQueue : DefaultBackgroundTaskQueue
{
    private readonly Channel<IBackgroundTask> tasks = Channel.CreateUnbounded<IBackgroundTask>();
    private readonly IList<IBackgroundTaskQueueFilter> filters;
    public InMemoryBackgroundTaskQueue(IEnumerable<IBackgroundTaskQueueFilter> filters) : base(filters)
    {
        this.filters = filters.ToList();
    }

    public override async Task Enqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default)
    {
        await this.BeforeEnqueue(backgroundTask, cancellationToken);
        if (!tasks.Writer.TryWrite(backgroundTask))
        {
            throw new InvalidOperationException();
        }
    }

    public override async Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default)
    {
        var backgroundTask = await tasks.Reader.ReadAsync(cancellationToken);
        return backgroundTask;
    }
}
