using Daemon.AppConfig;

namespace Daemon;

public class WorkerBackgroundService(ILogger<WorkerBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = AppSettings.EmailCheckFrequency!.Value * 60_000;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(interval, stoppingToken);
        }
    }
}
