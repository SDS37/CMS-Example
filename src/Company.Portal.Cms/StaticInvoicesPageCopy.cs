namespace Company.Portal.Cms;

internal sealed class StaticInvoicesPageCopy : ILoadInvoicesPageCopy
{
    public Task<InvoicesPageCopy?> Load(string language, CancellationToken cancellationToken)
    {
        if (language == "en")
        {
            return Task.FromResult<InvoicesPageCopy?>(new InvoicesPageCopy(
                "My invoices",
                "<p>Open and recently paid invoices.</p>",
                "<p>Contact billing if an amount looks wrong.</p>",
                "en"));
        }

        return Task.FromResult<InvoicesPageCopy?>(new InvoicesPageCopy(
            "Mina fakturor",
            "<p>Öppna och nyligen betalda fakturor.</p>",
            "<p>Kontakta ekonomi om ett belopp ser fel ut.</p>",
            "sv"));
    }
}
