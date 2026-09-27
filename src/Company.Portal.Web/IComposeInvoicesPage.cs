namespace Company.Portal.Web;

public interface IComposeInvoicesPage
{
    Task<InvoicesPageResponse?> Get(string oidcSubject, string language, CancellationToken cancellationToken);
}
