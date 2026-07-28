using Commons.QueuedBackgroundTasks.Abstractions.Middlewares;
using Commons.QueuedBackgroundTasks.Abstractions.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Commons.QueuedBackgroundTasks.Extensions;

public interface IBackgroundTaskServiceBuilder
{
    IServiceCollection Services { get; }
    IBackgroundTaskServiceBuilder UseHostedService<THostedService>() where THostedService : class, IHostedService;
    IBackgroundTaskServiceBuilder UseQueue<TQueue>() where TQueue : class, IBackgroundTaskQueue;
    IBackgroundTaskServiceBuilder UseFilter<TFilter>() where TFilter : class, IBackgroundTaskQueueFilter;
    IBackgroundTaskServiceBuilder AddBackgroundTaskHandlers(Assembly assembly, string? context = null);
    IBackgroundTaskServiceBuilder UseMiddleware<TMiddleware>() where TMiddleware : class, IBackgroundTaskMiddleware;
}
