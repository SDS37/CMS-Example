using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Company.Portal.Cms;

internal sealed class CachedInvoicesPageCopy(
    ILoadInvoicesPageCopy inner,
    IDistributedCache cache,
    ILogger<CachedInvoicesPageCopy> logger) : IProvideInvoicesPageCopy
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(2);
    private const string CacheKeyPrefix = "cms:invoices-page:";

    public async Task<InvoicesPageCopy?> Get(string language, CancellationToken cancellationToken)
    {
        string cacheKey = CacheKeyPrefix + language;
        InvoicesPageCopy? cached = await ReadCache(cacheKey, cancellationToken).ConfigureAwait(false);

        if (cached is not null)
        {
            return cached;
        }

        InvoicesPageCopy? copy = await inner.Load(language, cancellationToken).ConfigureAwait(false);

        if (copy is null)
        {
            logger.LogWarning("No published invoices page for language {Language}", language);
            return null;
        }

        await WriteCache(cacheKey, copy, cancellationToken).ConfigureAwait(false);
        return copy;
    }

    private async Task<InvoicesPageCopy?> ReadCache(string cacheKey, CancellationToken cancellationToken)
    {
        string? payload = await cache.GetStringAsync(cacheKey, cancellationToken).ConfigureAwait(false);

        if (payload is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<InvoicesPageCopy>(payload, SerializerOptions);
    }

    private Task WriteCache(string cacheKey, InvoicesPageCopy copy, CancellationToken cancellationToken)
    {
        string payload = JsonSerializer.Serialize(copy, SerializerOptions);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheLifetime
        };

        return cache.SetStringAsync(cacheKey, payload, options, cancellationToken);
    }
}
