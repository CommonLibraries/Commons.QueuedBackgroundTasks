namespace Commons.QueuedBackgroundTasks.Abstractions;

public interface IBackgroundTaskQueue
{
    Task Queue(IBackgroundTask backgroundTask, CancellationToken cancellationToken = default);
    Task<IBackgroundTask> Dequeue(CancellationToken cancellationToken = default);
}
