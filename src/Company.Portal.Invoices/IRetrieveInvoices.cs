namespace Company.Portal.Invoices;

internal interface IRetrieveInvoices
{
    Task<IReadOnlyList<Invoice>> ListRecent(CustomerSubject subject, CancellationToken cancellationToken);
}
