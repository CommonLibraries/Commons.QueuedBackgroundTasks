using System.Reflection;

namespace Commons.QueuedBackgroundTasks.Extensions;

public interface IBackgroundTaskServiceBuilder
{
    IBackgroundTaskServiceBuilder AddBackgroundTaskHandlers(Assembly assembly, string? context = null);
}
