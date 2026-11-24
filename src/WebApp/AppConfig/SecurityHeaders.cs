namespace WebApp.AppConfig;

internal static class SecurityHeaders
{
    public static void AddHttpSecurity(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddHttpsRedirection(options =>
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect);

            return;
        }

        builder.Services
            .AddHsts(options => options.MaxAge = TimeSpan.FromDays(730))
            .AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            })
            .AddAntiforgery(options => options.Cookie.SecurePolicy = CookieSecurePolicy.Always);
    }
}
