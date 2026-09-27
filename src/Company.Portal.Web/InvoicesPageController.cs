using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Portal.Web;

[ApiController]
[Authorize]
[Route("api/me/invoices-page")]
public sealed class InvoicesPageController(IComposeInvoicesPage composer) : ControllerBase
{
    private const string SubjectClaim = "sub";
    private const string EnglishPrefix = "en";
    private const string English = "en";
    private const string Swedish = "sv";

    [HttpGet]
    [ProducesResponseType(typeof(InvoicesPageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoicesPageResponse>> Get(CancellationToken cancellationToken)
    {
        string? subject = User.FindFirstValue(SubjectClaim) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return Unauthorized();
        }

        string language = ResolveLanguage(Request.Headers.AcceptLanguage.ToString());
        InvoicesPageResponse? page = await composer.Get(subject, language, cancellationToken);

        if (page is null)
        {
            return NotFound();
        }

        return Ok(page);
    }

    private static string ResolveLanguage(string acceptLanguage)
    {
        if (acceptLanguage.StartsWith(EnglishPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return English;
        }

        return Swedish;
    }
}
