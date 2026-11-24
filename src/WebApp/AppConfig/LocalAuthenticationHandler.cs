using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApp.AppConfig;

// Provides an authenticated user when running locally
internal class LocalAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string BasicAuthenticationScheme = "BasicAuthentication";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        const string userEmail = "example@example.org";
        if (string.IsNullOrEmpty(userEmail))
            return Task.FromResult(AuthenticateResult.Fail("Invalid"));
        var claims = new[] { new Claim(ClaimTypes.Name, userEmail) };
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name)), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);
        await Response.BodyWriter.WriteAsync(
            "Status Code: 403; Forbidden \r\n To access protected pages, set 'DevOptions.AuthenticatedUser' to an email address in 'appsettings.Development.json' file."u8
                .ToArray());
    }
}
