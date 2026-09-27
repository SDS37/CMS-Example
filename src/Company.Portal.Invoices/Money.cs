namespace Company.Portal.Invoices;

internal readonly record struct Money
{
    public const string SwedishKrona = "SEK";

    public Money(decimal amount, string currency)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public static Money InKronor(decimal amount) => new(amount, SwedishKrona);
}
