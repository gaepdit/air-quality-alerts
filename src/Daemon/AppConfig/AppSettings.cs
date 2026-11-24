using Azure.Identity;
using System.Reflection;

namespace Daemon.AppConfig;

internal static class AppSettings
{
    public static string? Version { get; private set; }

    /// <summary>
    /// The frequency to check for new air quality monitor emails in minutes.
    /// </summary>
    public static int? EmailCheckFrequency { get; private set; }

    public static void LoadAppSettings(this IHostApplicationBuilder builder)
    {
        // Set default timeout for regular expressions.
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
        AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

        Version = GetVersion();
        builder.AddKeyVault();
        EmailCheckFrequency = builder.Configuration.GetValue<int?>(nameof(EmailCheckFrequency)) ??
                              throw new InvalidOperationException("Email frequency not configured");
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }

    private static void AddKeyVault(this IHostApplicationBuilder builder)
    {
        var keyVaultName = builder.Configuration.GetValue<string>("AzureKeyVaultName");
        if (string.IsNullOrEmpty(keyVaultName)) return;

        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");
        builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
    }
}
