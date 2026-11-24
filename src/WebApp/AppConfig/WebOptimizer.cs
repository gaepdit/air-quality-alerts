namespace WebApp.AppConfig;

internal static class WebOptimizer
{
    public static void AddWebOptimizer(this IHostApplicationBuilder builder)
    {
        var isDevelopment = builder.Environment.IsDevelopment();
        builder.Services.AddWebOptimizer(minifyJavaScript: !isDevelopment, minifyCss: !isDevelopment);
    }
}
