using Commons.QueuedBackgroundTasks.Abstractions.Contexts;

namespace Commons.QueuedBackgroundTasks.Extensions.Contexts;

internal class DefaultBackgroundTaskContextLookup : IBackgroundTaskContextLookup
{
    private readonly IDictionary<Type, string> contexts;
    public DefaultBackgroundTaskContextLookup(IDictionary<Type, string> contexts)
    {
        this.contexts = contexts;
    }

    public string? Get(Type handlerType)
    {
        if (this.contexts.TryGetValue(handlerType, out var context))
        {
            return context;
        }

        return null;
    }
}
