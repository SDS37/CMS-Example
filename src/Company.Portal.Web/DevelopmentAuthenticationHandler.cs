using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Company.Portal.Web;

internal sealed class DevelopmentAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Development";
    public const string UserHeader = "X-User-Sub";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string subject = Request.Headers[UserHeader].ToString();

        if (string.IsNullOrWhiteSpace(subject))
        {
            subject = "user-1";
        }

        var identity = new ClaimsIdentity(
            [new Claim("sub", subject), new Claim(ClaimTypes.NameIdentifier, subject)],
            SchemeName);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
