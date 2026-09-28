using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Distributed;

namespace Company.Portal.Web;

internal sealed class DistributedCacheTicketStore(
    IDistributedCache cache,
    IDataProtectionProvider protection) : ITicketStore
{
    private const string Purpose = "Company.Portal.Web.BffSession";
    private const string KeyPrefix = "bff:session:";
    private static readonly TimeSpan Lifetime = TimeSpan.FromHours(8);
    private readonly IDataProtector protector = protection.CreateProtector(Purpose);

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        string key = Guid.NewGuid().ToString("N");
        await RenewAsync(key, ticket).ConfigureAwait(false);
        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        byte[] payload = protector.Protect(TicketSerializer.Default.Serialize(ticket));
        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = Lifetime
        };

        await cache.SetAsync(KeyPrefix + key, payload, options).ConfigureAwait(false);
    }

    public async Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        byte[]? protectedPayload = await cache.GetAsync(KeyPrefix + key).ConfigureAwait(false);

        if (protectedPayload is null)
        {
            return null;
        }

        try
        {
            byte[] payload = protector.Unprotect(protectedPayload);
            return TicketSerializer.Default.Deserialize(payload);
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            return null;
        }
    }

    public Task RemoveAsync(string key)
    {
        return cache.RemoveAsync(KeyPrefix + key);
    }
}
