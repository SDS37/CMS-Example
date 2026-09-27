using Microsoft.EntityFrameworkCore;

namespace Company.Portal.Invoices;

internal sealed class SqlInvoiceStore(InvoicesDbContext database, TimeProvider clock) : IRetrieveInvoices
{
    private static readonly TimeSpan Retention = TimeSpan.FromDays(365);

    public async Task<IReadOnlyList<Invoice>> ListRecent(
        CustomerSubject subject,
        CancellationToken cancellationToken)
    {
        DateOnly cutoff = DateOnly.FromDateTime(clock.GetUtcNow().UtcDateTime.Subtract(Retention));

        List<InvoiceRecord> rows = await database.Invoices
            .AsNoTracking()
            .Where(record => record.Subject == subject.Value && record.DueDate >= cutoff)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return rows.Select(ToInvoice).ToArray();
    }

    private static Invoice ToInvoice(InvoiceRecord record)
    {
        var snapshot = new InvoiceSnapshot(
            new InvoiceId(record.Id),
            new CustomerSubject(record.Subject),
            new Money(record.Amount, record.Currency),
            record.DueDate,
            (InvoiceStatus)record.Status,
            record.PaidAt);

        return new Invoice(snapshot);
    }
}
