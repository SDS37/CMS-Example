using Xunit;

namespace Company.Portal.Invoices.Tests;

public sealed class InvoiceTests
{
    [Fact]
    public void OpenInvoice_AfterDueDate_IsOverdue()
    {
        Invoice invoice = Create(InvoiceStatus.Open, new DateOnly(2026, 1, 1), paidAt: null);

        bool overdue = invoice.IsOverdue(new DateOnly(2026, 1, 2));

        Assert.True(overdue);
    }

    [Fact]
    public void OpenInvoice_OnDueDate_IsNotOverdue()
    {
        var dueDate = new DateOnly(2026, 1, 2);
        Invoice invoice = Create(InvoiceStatus.Open, dueDate, paidAt: null);

        bool overdue = invoice.IsOverdue(dueDate);

        Assert.False(overdue);
    }

    [Fact]
    public void PaidInvoice_WithoutTimestamp_CannotBeCreated()
    {
        Assert.Throws<ArgumentException>(() =>
            Create(InvoiceStatus.Paid, new DateOnly(2026, 1, 1), paidAt: null));
    }

    private static Invoice Create(InvoiceStatus status, DateOnly dueDate, DateTimeOffset? paidAt)
    {
        var snapshot = new InvoiceSnapshot(
            InvoiceId.New(),
            new CustomerSubject("user-1"),
            Money.InKronor(100m),
            dueDate,
            status,
            paidAt);

        return new Invoice(snapshot);
    }
}
