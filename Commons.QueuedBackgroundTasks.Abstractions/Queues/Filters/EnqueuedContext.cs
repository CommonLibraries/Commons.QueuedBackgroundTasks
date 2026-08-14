namespace Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

public class EnqueuedContext
{
    public required CancellationToken CancellationToken { get; init; }
}