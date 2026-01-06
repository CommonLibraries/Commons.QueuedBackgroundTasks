using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.QueuedBackgroundTasks.Extensions;

internal class DefaultBackgroundTaskDispatcher : IBackgroundTaskDispatcher
{
    private readonly IServiceProvider serviceProvider;
    public DefaultBackgroundTaskDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task Execute<TBackgroundTask>(TBackgroundTask backgroundTask, CancellationToken cancellationToken = default) where TBackgroundTask : IBackgroundTask
    {
        var backgroundTaskHandler = this.serviceProvider.GetRequiredService<IBackgroundTaskHandler<TBackgroundTask>>();
        await backgroundTaskHandler.Handle(backgroundTask, cancellationToken);
    }
}
