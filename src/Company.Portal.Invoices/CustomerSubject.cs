namespace Company.Portal.Invoices;

internal readonly record struct CustomerSubject
{
    public CustomerSubject(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public string Value { get; }
}
