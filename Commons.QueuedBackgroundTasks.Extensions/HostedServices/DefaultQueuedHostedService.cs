using Commons.QueuedBackgroundTasks.Abstractions;
using Commons.QueuedBackgroundTasks.Abstractions.Contexts;
using Commons.QueuedBackgroundTasks.Abstractions.Middlewares;
using Commons.QueuedBackgroundTasks.Abstractions.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Commons.QueuedBackgroundTasks.Extensions.HostedServices;

/// <summary>
/// A hosted service (long running service) that processes background tasks from a queue.
/// </summary>
public abstract class DefaultQueuedHostedService : BackgroundService
{
    protected readonly IBackgroundTaskQueue backgroundTaskQueue;
    protected readonly IServiceScopeFactory serviceScopeFactory;
    protected readonly IBackgroundTaskHandlerLookup backgroundTaskHandlerLookup;
    protected readonly IBackgroundTaskContextLookup backgroundTaskContextLookup;
    protected readonly ILogger<DefaultQueuedHostedService> logger;
    public DefaultQueuedHostedService(
        IBackgroundTaskQueue backgroundTaskQueue,
        IServiceScopeFactory serviceScopeFactory,
        IBackgroundTaskHandlerLookup backgroundTaskHandlerLookup,
        IBackgroundTaskContextLookup backgroundTaskContextLookup,
        ILogger<DefaultQueuedHostedService> logger)
    {
        this.backgroundTaskQueue = backgroundTaskQueue;
        this.serviceScopeFactory = serviceScopeFactory;
        this.backgroundTaskHandlerLookup = backgroundTaskHandlerLookup;
        this.backgroundTaskContextLookup = backgroundTaskContextLookup;
        this.logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Starting background task runner.");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Stopping background task runner.");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Starting to process queued background tasks.");
        await ProcessTasks(stoppingToken);
    }

    protected virtual Task Prepare(
        string? context,
        IServiceProvider serviceProvider,
        Func<Task> next,
        CancellationToken cancellationToken)
    {
        return next();
    }

    protected async Task ProcessTasks(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var backgroundTask = await backgroundTaskQueue.Dequeue(cancellationToken);
            try
            {
                await using (var scope = this.serviceScopeFactory.CreateAsyncScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var backgroundTaskDispatcher = serviceProvider.GetRequiredService<IBackgroundTaskDispatcher>();

                    Func<Task> pipeline = async () =>
                    {
                        var dispatcherType = backgroundTaskDispatcher.GetType();
                        var dispatcherExecuteMethod = dispatcherType?.GetMethod(nameof(IBackgroundTaskDispatcher.Execute));
                        if (dispatcherExecuteMethod is null)
                            throw new NotImplementedException();

                        var task = dispatcherExecuteMethod?.MakeGenericMethod(backgroundTask.GetType()).Invoke(backgroundTaskDispatcher, new object[] { backgroundTask, cancellationToken }) as Task;
                        if (task is null)
                            throw new NotImplementedException();

                        await task;
                    };

                    var handlerType = this.backgroundTaskHandlerLookup.Get(backgroundTask.GetType());
                    var context = handlerType is null ?
                        null : this.backgroundTaskContextLookup.Get(handlerType);

                    var middlewares = serviceProvider.GetServices<IBackgroundTaskMiddleware>();
                    var middlewareContext = new BackgroundTaskMiddlewareContext()
                    {
                        Context = context,
                        CancellationToken = cancellationToken
                    };

                    foreach (var middleware in middlewares.Reverse())
                    {
                        var next = pipeline;
                        pipeline = async () =>
                        {
                            await middleware.Invoke(backgroundTask, middlewareContext, next);
                        };
                    }

                    await this.Prepare(
                        context,
                        serviceProvider,
                        pipeline,
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "There is an error while executing the background task.");
            }
        }
    }
}
