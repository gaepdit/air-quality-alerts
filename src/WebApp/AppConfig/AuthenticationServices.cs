using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.AppConfig;

public static class AuthenticationServices
{
    public static void ConfigureAuthentication(this IHostApplicationBuilder builder)
    {
        var local = builder.Configuration.GetValue<bool>("UseLocalAuth");

        if (local)
        {
            // When running locally, use a built-in authenticated user.
            builder.Services
                .AddAuthentication(LocalAuthenticationHandler.BasicAuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, LocalAuthenticationHandler>(
                    LocalAuthenticationHandler.BasicAuthenticationScheme, null);
        }
        else
        {
            // When running on the server, require an OIDC login provider (configured in the app settings file).
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(configureOptions: options =>
                {
                    var configSection = builder.Configuration.GetSection("OIDC");

                    options.Authority = configSection["Authority"];
                    options.ClientId = configSection["ClientId"];
                    options.ClientSecret = configSection["ClientSecret"];
                    options.CallbackPath = configSection["CallbackPath"];

                    options.Scope.Add("email");
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "email" };
                });
        }
    }
}
