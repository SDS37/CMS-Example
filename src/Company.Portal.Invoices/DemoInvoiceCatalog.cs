using Microsoft.EntityFrameworkCore;

namespace Company.Portal.Invoices;

public static class DemoInvoiceCatalog
{
    public const string DefaultSubject = "user-1";

    public static async Task Seed(InvoicesDbContext database, CancellationToken cancellationToken)
    {
        if (await database.Invoices.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        database.Invoices.AddRange(
            new InvoiceRecord
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                Subject = DefaultSubject,
                Amount = 1250.50m,
                Currency = Money.SwedishKrona,
                DueDate = new DateOnly(2026, 1, 15),
                Status = (int)InvoiceStatus.Open
            },
            new InvoiceRecord
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                Subject = DefaultSubject,
                Amount = 430.00m,
                Currency = Money.SwedishKrona,
                DueDate = new DateOnly(2026, 10, 1),
                Status = (int)InvoiceStatus.Open
            },
            new InvoiceRecord
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                Subject = DefaultSubject,
                Amount = 890.00m,
                Currency = Money.SwedishKrona,
                DueDate = new DateOnly(2025, 12, 1),
                Status = (int)InvoiceStatus.Paid,
                PaidAt = new DateTimeOffset(2025, 12, 2, 0, 0, 0, TimeSpan.Zero)
            });

        await database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
