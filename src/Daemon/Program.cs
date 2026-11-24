using Daemon;
using Daemon.AppConfig;
using Daemon.TaskQueue;

var builder = Host.CreateApplicationBuilder(args);

// Configure basic settings
builder.LoadAppSettings();
builder.Services.AddWindowsService(opts => opts.ServiceName = "Air Quality Alerts Notification Service");

// Register services
builder.Services.AddHostedService<WorkerBackgroundService>();
builder.Services.AddHostedService<QueueBackgroundService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(_ =>
{
    var queueCapacity = builder.Configuration.GetValue<int?>("QueueCapacity") ?? 100;
    return new BackgroundTaskQueue(queueCapacity);
});

// Build and run
var host = builder.Build();
await host.RunAsync();
