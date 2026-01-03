using Commons.QueuedBackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Commons.QueuedBackgroundTasks.Extensions;

public abstract class BackgroundTaskRunnerHostedService : BackgroundService
{
    protected readonly IBackgroundTaskQueue backgroundTaskQueue;
    protected readonly IServiceScopeFactory serviceScopeFactory;
    protected readonly ILogger<BackgroundTaskRunnerHostedService> logger;
    public BackgroundTaskRunnerHostedService(
        IBackgroundTaskQueue backgroundTaskQueue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<BackgroundTaskRunnerHostedService> logger)
    {
        this.backgroundTaskQueue = backgroundTaskQueue;
        this.serviceScopeFactory = serviceScopeFactory;
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

    protected abstract Task Prepare(Func<Task> next);

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

                    Func<Task> next = async () =>
                    {
                        var dispatcherType = backgroundTaskDispatcher.GetType();
                        var dispatcherExecuteMethod = dispatcherType?.GetMethod(nameof(IBackgroundTaskDispatcher.Execute));
                        if (dispatcherExecuteMethod is null)
                            throw new NotImplementedException();

                        var task = dispatcherExecuteMethod?.MakeGenericMethod(backgroundTask.GetType()).Invoke(backgroundTaskDispatcher, [backgroundTask, cancellationToken]) as Task;
                        if (task is null)
                            throw new NotImplementedException();

                        await task;
                    };

                    await this.Prepare(next);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "There is an error while executing the background task.");
            }
        }
    }
}
