namespace Commons.QueuedBackgroundTasks.Abstractions;

/// <summary>
/// This interface provide context key for a background task handler.
/// </summary>
public interface IBackgroundTaskContextLookup
{
    string? Get(Type handlerType);
}
