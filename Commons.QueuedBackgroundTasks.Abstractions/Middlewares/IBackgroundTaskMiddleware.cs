namespace Commons.QueuedBackgroundTasks.Abstractions.Middlewares;

public interface IBackgroundTaskMiddleware
{
    Task Invoke(IBackgroundTask backgroundTask, BackgroundTaskMiddlewareContext context, Func<Task> next);
}
