using System;

namespace Commons.QueuedBackgroundTasks.Abstractions;

/// <summary>
/// This interface exists alongside with Dependency Injection container
/// to provide mapping between background task and its handler (1-to-1 mapping).
/// </summary>
public interface IBackgroundTaskHandlerLookup
{
    Type? Get(Type task);
}
