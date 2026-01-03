namespace Commons.QueuedBackgroundTasks.Abstractions;

public interface IBackgroundTaskHandler<TBackgroundTask> where TBackgroundTask : IBackgroundTask
{
    Task Handle(TBackgroundTask backgroundTask, CancellationToken cancellationToken = default);
}
