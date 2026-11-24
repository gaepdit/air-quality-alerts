using WebApp.AppConfig;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders().AddZLoggerConsole(options => options.UseJsonFormatter());

// Configure basic settings
builder.LoadAppSettings().AddHttpSecurity();
builder.Services.AddDataProtection();

// Configure authentication and authorization
builder.ConfigureAuthentication();
builder.Services.AddAuthorization();

// Configure the UI
builder.Services.AddRazorPages();
builder.AddWebOptimizer();

// Build the application
var app = builder.Build();

// Configure error handling.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
else app.UseExceptionHandler("/Error"); // Production or Staging

// Configure the HTTP request pipeline
app
    .UseStatusCodePages()
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
