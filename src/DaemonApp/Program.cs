using DaemonApp;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService();
builder.Services.AddHostedService<WorkerBackgroundService>();

var host = builder.Build();
await host.RunAsync();
