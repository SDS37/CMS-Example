namespace Company.Portal.Invoices;

internal sealed class InvoiceRecord
{
    public Guid Id { get; set; }

    public string Subject { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = Money.SwedishKrona;

    public DateOnly DueDate { get; set; }

    public int Status { get; set; }

    public DateTimeOffset? PaidAt { get; set; }
}
