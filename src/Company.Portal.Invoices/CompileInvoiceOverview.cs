namespace Company.Portal.Invoices;

internal sealed class CompileInvoiceOverview(IRetrieveInvoices invoices) : ICompileInvoiceOverview
{
    public async Task<IReadOnlyList<InvoiceOverviewItem>> Get(
        string oidcSubject,
        DateOnly today,
        CancellationToken cancellationToken)
    {
        var subject = new CustomerSubject(oidcSubject);
        IReadOnlyList<Invoice> invoicesForSubject =
            await invoices.ListRecent(subject, cancellationToken).ConfigureAwait(false);

        return
        [
            .. invoicesForSubject
                .OrderBy(invoice => invoice.Snapshot.DueDate)
                .Select(invoice => ToItem(invoice, today)),
        ];
    }

    private static InvoiceOverviewItem ToItem(Invoice invoice, DateOnly today)
    {
        InvoiceSnapshot snapshot = invoice.Snapshot;

        return new InvoiceOverviewItem(
            snapshot.Id.Value,
            snapshot.Amount.Amount,
            snapshot.Amount.Currency,
            snapshot.DueDate,
            snapshot.Status.ToString(),
            invoice.IsOverdue(today));
    }
}
