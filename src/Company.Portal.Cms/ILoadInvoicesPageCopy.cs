namespace Company.Portal.Cms;

internal interface ILoadInvoicesPageCopy
{
    Task<InvoicesPageCopy?> Load(string language, CancellationToken cancellationToken);
}
