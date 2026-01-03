using System.Threading.Channels;
using Commons.QueuedBackgroundTasks.Abstractions;

namespace Commons.QueuedBackgroundTasks.InMemoryBackgroundTaskQueue;

public class InMemoryBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<IBackgroundTask> tasks = Channel.CreateUnbounded<IBackgroundTask>();

    public Task Queue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default)
    {
        if (!tasks.Writer.TryWrite(backgroundTask))
        {
            throw new InvalidOperationException();
        }

        return Task.CompletedTask;
    }

    public async Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default)
    {
        var backgroundTask = await tasks.Reader.ReadAsync(cancellationToken);
        return backgroundTask;
    }
}
