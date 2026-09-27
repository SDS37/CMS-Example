using Company.Portal.Cms;
using Company.Portal.Invoices;

namespace Company.Portal.Web;

internal sealed class InvoicesPageComposer(
    IProvideInvoicesPageCopy pageCopy,
    ICompileInvoiceOverview invoices,
    TimeProvider clock) : IComposeInvoicesPage
{
    public async Task<InvoicesPageResponse?> Get(
        string oidcSubject,
        string language,
        CancellationToken cancellationToken)
    {
        DateOnly today = DateOnly.FromDateTime(clock.GetUtcNow().UtcDateTime);

        Task<InvoicesPageCopy?> copyTask = pageCopy.Get(language, cancellationToken);
        Task<IReadOnlyList<InvoiceOverviewItem>> invoicesTask = invoices.Get(oidcSubject, today, cancellationToken);

        await Task.WhenAll(copyTask, invoicesTask).ConfigureAwait(false);

        InvoicesPageCopy? copy = await copyTask.ConfigureAwait(false);

        if (copy is null)
        {
            return null;
        }

        return new InvoicesPageResponse(
            copy.Heading,
            copy.IntroductionHtml,
            copy.HelpHtml,
            await invoicesTask.ConfigureAwait(false));
    }
}
