using Daemon;
using Daemon.AppConfig;

var builder = Host.CreateApplicationBuilder(args);

// Configure basic settings
builder.LoadAppSettings();

// Register workers
builder.Services.AddWindowsService(opts => opts.ServiceName = "Air Quality Alerts Notification Service");
builder.Services.AddHostedService<WorkerBackgroundService>();

// Build and run
var host = builder.Build();
await host.RunAsync();
