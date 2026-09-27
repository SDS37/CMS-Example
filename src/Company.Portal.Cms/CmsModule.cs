using Microsoft.Extensions.DependencyInjection;

namespace Company.Portal.Cms;

public static class CmsModule
{
    public static IServiceCollection AddCmsModule(this IServiceCollection services)
    {
        services.AddSingleton<ILoadInvoicesPageCopy, StaticInvoicesPageCopy>();
        services.AddScoped<IProvideInvoicesPageCopy, CachedInvoicesPageCopy>();
        return services;
    }
}
