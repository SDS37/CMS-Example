using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Portal.Invoices;

public static class InvoiceModule
{
    public static IServiceCollection AddInvoiceModule(
        this IServiceCollection services,
        string? sqlConnectionString)
    {
        if (string.IsNullOrWhiteSpace(sqlConnectionString))
        {
            services.AddDbContext<InvoicesDbContext>(options =>
                options.UseInMemoryDatabase("portal-invoices"));
        }
        else
        {
            services.AddDbContext<InvoicesDbContext>(options =>
                options.UseSqlServer(sqlConnectionString));
        }

        services.AddScoped<IRetrieveInvoices, SqlInvoiceStore>();
        services.AddScoped<ICompileInvoiceOverview, CompileInvoiceOverview>();
        return services;
    }
}
