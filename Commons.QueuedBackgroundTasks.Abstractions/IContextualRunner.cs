namespace Commons.QueuedBackgroundTasks.Abstractions;

public interface IContextualRunner
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
