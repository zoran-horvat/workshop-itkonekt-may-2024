namespace Bookstore.Domain.Common;

public readonly record struct Money : IComparable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money Zero => new(0, Currency.Empty);

    public Money() : this(0, Currency.Empty) { }

    public Money(decimal amount, Currency currency)
    {
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }

    public bool IsZero => Amount == 0 && Currency == Currency.Empty;

    public Money Add(Money other) =>
        this.IsZero ? other
        : other.IsZero ? this
        : this.Currency == other.Currency ? new Money(this.Amount + other.Amount, this.Currency)
        : throw new InvalidOperationException("Cannot add money of different currencies");

    public Money Subtract(Money other) =>
        other.IsZero ? this
        : this.Currency == other.Currency && this.Amount >= other.Amount ? new Money(this.Amount - other.Amount, this.Currency)
        : this.Currency == other.Currency ? throw new InvalidOperationException("Cannot subtract more money than available")
        : throw new InvalidOperationException("Cannot subtract money of different currencies");

    public Money Scale(decimal factor) =>
        factor < 0 ? throw new InvalidOperationException("Cannot multiply by a negative factor")
        : new Money(Amount * factor, Currency);

    public int CompareTo(Money other) =>
        this.IsZero && other.IsZero ? 0
        : this.IsZero ? -1
        : other.IsZero ? 1
        : this.Currency == other.Currency ? Amount.CompareTo(other.Amount)
        : throw new InvalidOperationException("Cannot compare money of different currencies");

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, decimal right) => left.Scale(right);
    public static Money operator *(decimal left, Money right) => right.Scale(left);

    public override string ToString() => $"{Amount:0.00} {Currency.Symbol}";
}