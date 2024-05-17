namespace Bookstore.Domain.Common;

public record struct Currency(string Symbol)
{
    public override string ToString() => Symbol;

    public Currency() : this(string.Empty) { }

    internal static Currency Empty => new(string.Empty);

    public static readonly Currency EUR = new("EUR");
    public static readonly Currency USD = new("USD");

    public Money Amount(decimal amount) => new(amount, this);
}