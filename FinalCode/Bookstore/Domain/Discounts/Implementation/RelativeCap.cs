using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public sealed class RelativeCap(IDiscount other, decimal factor) : Cap(other)
{
    private readonly decimal _factor =
        factor >= 0 && factor <= 1 ? factor
        : throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0 and 1");

    protected override Money GetCapAmount(Money originalPrice) =>
        originalPrice * _factor;
}