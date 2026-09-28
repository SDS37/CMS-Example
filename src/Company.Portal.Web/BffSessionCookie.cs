using Microsoft.AspNetCore.Authentication.Cookies;

namespace Company.Portal.Web;

internal static class BffSessionCookie
{
    public const string Name = "__Host-bff";
    public static readonly TimeSpan Lifetime = TimeSpan.FromHours(8);

    public static void Configure(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = Name;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Path = "/";
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = Lifetime;
        options.SlidingExpiration = true;
    }
}
