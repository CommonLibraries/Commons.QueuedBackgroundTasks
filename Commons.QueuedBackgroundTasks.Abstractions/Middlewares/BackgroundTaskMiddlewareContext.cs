namespace Commons.QueuedBackgroundTasks.Abstractions.Middlewares;

public class BackgroundTaskMiddlewareContext
{
    public required string? Context { get; init; }
    public required CancellationToken CancellationToken { get; init; }
}
