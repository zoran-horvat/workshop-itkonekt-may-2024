using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public sealed class AbsoluteCap(IDiscount other, Money amount) : Cap(other)
{
    private readonly Money _cap = amount;

    protected override Money GetCapAmount(Money originalPrice) => _cap;
}
