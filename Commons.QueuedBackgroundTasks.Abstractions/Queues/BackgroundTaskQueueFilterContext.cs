namespace Commons.QueuedBackgroundTasks.Abstractions.Queues;

public class BackgroundTaskQueueFilterContext
{
    public bool IsEnqueueAllowed { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
