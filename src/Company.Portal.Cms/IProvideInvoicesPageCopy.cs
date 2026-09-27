namespace Company.Portal.Cms;

public interface IProvideInvoicesPageCopy
{
    Task<InvoicesPageCopy?> Get(string language, CancellationToken cancellationToken);
}
