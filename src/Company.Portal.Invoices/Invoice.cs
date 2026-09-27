namespace Company.Portal.Invoices;

internal sealed class Invoice
{
    public Invoice(InvoiceSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (IsPaidWithoutTimestamp(snapshot))
        {
            throw new ArgumentException("A paid invoice must have a payment timestamp.");
        }

        Snapshot = snapshot;
    }

    public InvoiceSnapshot Snapshot { get; }

    public bool IsOverdue(DateOnly today)
    {
        return IsOpen() && Snapshot.DueDate < today;
    }

    private bool IsOpen()
    {
        return Snapshot.Status == InvoiceStatus.Open;
    }

    private static bool IsPaidWithoutTimestamp(InvoiceSnapshot snapshot)
    {
        return snapshot.Status == InvoiceStatus.Paid && snapshot.PaidAt is null;
    }
}
