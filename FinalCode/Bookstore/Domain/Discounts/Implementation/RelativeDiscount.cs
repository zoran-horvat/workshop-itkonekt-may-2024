using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public sealed class RelativeDiscount(decimal factor) : IDiscount
{
    private decimal _factor =
        factor <= 0 || factor >= 1
            ? throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0 and 1")
            : factor;

    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice) =>
        [ new ($"Discount {_factor:P2}", originalPrice * _factor) ];
}
