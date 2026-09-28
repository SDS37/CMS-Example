using System.Security.Claims;
using Company.Portal.Invoices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Portal.Web;

[ApiController]
[Route("bff")]
public sealed class BffSessionController(IHostEnvironment environment) : ControllerBase
{
    public const string UserHeader = "X-User-Sub";

    [HttpGet("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromQuery] string? returnUrl)
    {
        string target = LocalReturnPath.Normalize(returnUrl);

        if (!environment.IsDevelopment())
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = target
            };

            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        string subject = Request.Headers[UserHeader].ToString();

        if (string.IsNullOrWhiteSpace(subject))
        {
            subject = DemoInvoiceCatalog.DefaultSubject;
        }

        var identity = new ClaimsIdentity(
            [new Claim("sub", subject), new Claim(ClaimTypes.NameIdentifier, subject)],
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Redirect(target);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}
