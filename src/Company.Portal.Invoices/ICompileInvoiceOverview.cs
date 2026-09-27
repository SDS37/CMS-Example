namespace Company.Portal.Invoices;

public interface ICompileInvoiceOverview
{
    Task<IReadOnlyList<InvoiceOverviewItem>> Get(
        string oidcSubject,
        DateOnly today,
        CancellationToken cancellationToken);
}
