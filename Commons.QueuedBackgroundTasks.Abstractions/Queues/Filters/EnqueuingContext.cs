namespace Commons.QueuedBackgroundTasks.Abstractions.Queues.Filters;

public class EnqueuingContext
{
    public bool IsEnqueueAllowed { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
