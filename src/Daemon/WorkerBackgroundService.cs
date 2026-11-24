using Daemon.AppConfig;
using Daemon.TaskQueue;

namespace Daemon;

public class WorkerBackgroundService(IBackgroundTaskQueue taskQueue, ILogger<WorkerBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = AppSettings.EmailCheckFrequency!.Value;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // TODO: Replace with method that checks the mailbox and queues each email as
            //       an Email Processing Task.
            await taskQueue.QueueBackgroundWorkItemAsync(BuildTestWorkItemAsync);

            await Task.Delay(TimeSpan.FromMinutes(interval), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(WorkerBackgroundService)} is stopping.");
        await base.StopAsync(cancellationToken);
    }


    private async ValueTask BuildTestWorkItemAsync(CancellationToken token)
    {
        // Simulate a 5-second tasks to complete
        // for each enqueued work item

        var guid = Guid.NewGuid();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Queued work item {Guid} is starting.", guid);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(15), token);
        }
        catch (OperationCanceledException)
        {
            // Prevent throwing if the Delay is canceled
        }

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Queued work item {Guid} is complete.", guid);
    }
}
