using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Company.Portal.Web;

internal static class BffAuthentication
{
    public static IServiceCollection AddBffAuthentication(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddSingleton<ITicketStore, DistributedCacheTicketStore>();
        AuthenticationBuilder authentication = services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(BffSessionCookie.Configure);

        services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
            .Configure<ITicketStore>((options, store) => options.SessionStore = store);

        if (!environment.IsDevelopment())
        {
            authentication.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration["Auth:Authority"];
                options.ClientId = configuration["Auth:ClientId"];
                options.ClientSecret = configuration["Auth:ClientSecret"];
                    options.ResponseType = "code";
                options.UsePkce = true;
                options.SaveTokens = true;
                options.MapInboundClaims = false;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
        }

        return services;
    }
}
