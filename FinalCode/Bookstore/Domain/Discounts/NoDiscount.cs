using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public sealed class NoDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice) =>
        Enumerable.Empty<DiscountApplication>();
}