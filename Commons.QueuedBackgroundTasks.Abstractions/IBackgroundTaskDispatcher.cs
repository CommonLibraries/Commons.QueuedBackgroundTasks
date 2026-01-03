namespace Commons.QueuedBackgroundTasks.Abstractions;

public interface IBackgroundTaskDispatcher
{
    Task Execute<TBackgroundTask>(TBackgroundTask backgroundTask, CancellationToken cancellationToken = default)
                                where TBackgroundTask : IBackgroundTask;
}
