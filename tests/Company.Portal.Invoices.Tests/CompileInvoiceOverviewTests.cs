using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Company.Portal.Invoices.Tests;

public sealed class CompileInvoiceOverviewTests
{
    [Fact]
    public async Task Get_ReturnsOnlyInvoicesForSubject()
    {
        InvoicesDbContext database = CreateDatabase();
        await DemoInvoiceCatalog.Seed(database, CancellationToken.None);
        var clock = new FrozenClock(new DateTimeOffset(2026, 9, 27, 0, 0, 0, TimeSpan.Zero));
        var compiler = new CompileInvoiceOverview(new SqlInvoiceStore(database, clock));

        IReadOnlyList<InvoiceOverviewItem> items = await compiler.Get("user-1", new DateOnly(2026, 9, 27), CancellationToken.None);

        Assert.Equal(3, items.Count);
        Assert.Contains(items, item => item.IsOverdue);
        Assert.Contains(items, item => item.Status == "Paid");
    }

    private static InvoicesDbContext CreateDatabase()
    {
        var options = new DbContextOptionsBuilder<InvoicesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InvoicesDbContext(options);
    }

    private sealed class FrozenClock(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
