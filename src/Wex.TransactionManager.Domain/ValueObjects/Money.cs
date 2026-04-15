using Wex.TransactionManager.Domain.Exceptions;

namespace Wex.TransactionManager.Domain.ValueObjects;

public sealed record Money
{
    public decimal Value { get; }
    public string Currency { get; }

    private Money(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public static Money Usd(decimal value) => Create(value, "USD");

    public override string ToString() => $"{Currency} {Value:N2}";

    public static Money Zero(string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        return new Money(0, currency.ToUpperInvariant());
    }

    public static Money Create(decimal value, string currency = "USD")
    {
        if (value <= 0)
            throw new EntityValidationException("Value must be positive");
        
        if (string.IsNullOrWhiteSpace(currency))
            throw new EntityValidationException("Currency is required");

        var roundedValue = Math.Round(value, 2, MidpointRounding.AwayFromZero);

        return new Money(roundedValue, currency.ToUpperInvariant());
    }

}
