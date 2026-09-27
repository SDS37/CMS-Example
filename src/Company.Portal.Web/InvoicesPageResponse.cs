using Company.Portal.Invoices;

namespace Company.Portal.Web;

public sealed record InvoicesPageResponse(
    string Heading,
    string IntroductionHtml,
    string HelpHtml,
    IReadOnlyList<InvoiceOverviewItem> Invoices);
