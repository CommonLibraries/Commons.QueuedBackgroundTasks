namespace Commons.QueuedBackgroundTasks.Abstractions;

/// <summary>
/// This interface helps run a part of a background task handler in a specific context.
/// </summary>
public interface IBackgroundTaskContextualRunner
{
    Task Run(
        string contextKey,
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
    
    Task<T> Run<T>(
        string contextKey,
        Func<CancellationToken, Task<T>> action,
        CancellationToken cancellationToken = default);
}
