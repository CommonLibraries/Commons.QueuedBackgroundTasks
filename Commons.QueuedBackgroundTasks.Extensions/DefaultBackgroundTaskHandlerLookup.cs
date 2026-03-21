using Commons.QueuedBackgroundTasks.Abstractions;

namespace Commons.QueuedBackgroundTasks.Extensions;

internal class DefaultBackgroundTaskHandlerLookup : IBackgroundTaskHandlerLookup
{
    private readonly IDictionary<Type, Type> handlerTypes;
    public DefaultBackgroundTaskHandlerLookup(IDictionary<Type, Type> handlerTypes)
    {
        this.handlerTypes = handlerTypes;
    }

    public Type? Get(Type task)
    {
        if (this.handlerTypes.TryGetValue(task, out var handlerType))
        {
            return handlerType;
        }
        
        return null;
    }
}
