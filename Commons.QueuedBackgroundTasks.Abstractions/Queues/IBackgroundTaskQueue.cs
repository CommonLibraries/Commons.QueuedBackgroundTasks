namespace Commons.QueuedBackgroundTasks.Abstractions.Queues;

public interface IBackgroundTaskQueue
{
    Task Enqueue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default);
    Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default);
}
