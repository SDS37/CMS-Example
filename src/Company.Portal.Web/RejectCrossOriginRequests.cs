namespace Company.Portal.Web;

internal sealed class RejectCrossOriginRequests(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        if (IsSessionBound(context.Request.Path) && IsCrossOrigin(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        return next(context);
    }

    private static bool IsSessionBound(PathString path)
    {
        return path.StartsWithSegments("/api") || path.StartsWithSegments("/bff/logout");
    }

    private static bool IsCrossOrigin(HttpRequest request)
    {
        if (string.Equals(request.Headers["Sec-Fetch-Site"], "cross-site", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        string origin = request.Headers.Origin.ToString();

        if (string.IsNullOrEmpty(origin))
        {
            return false;
        }

        if (!Uri.TryCreate(origin, UriKind.Absolute, out Uri? originUri))
        {
            return true;
        }

        return !string.Equals(originUri.Host, request.Host.Host, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(originUri.Scheme, request.Scheme, StringComparison.OrdinalIgnoreCase);
    }
}
