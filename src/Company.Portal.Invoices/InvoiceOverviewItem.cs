namespace Company.Portal.Invoices;

public sealed record InvoiceOverviewItem(
    Guid Id,
    decimal Amount,
    string Currency,
    DateOnly DueDate,
    string Status,
    bool IsOverdue);
