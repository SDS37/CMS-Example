namespace Company.Portal.Invoices;

internal sealed record InvoiceSnapshot(
    InvoiceId Id,
    CustomerSubject Subject,
    Money Amount,
    DateOnly DueDate,
    InvoiceStatus Status,
    DateTimeOffset? PaidAt);
