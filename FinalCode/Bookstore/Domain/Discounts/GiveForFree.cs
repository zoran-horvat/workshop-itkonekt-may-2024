using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public sealed class GiveForFree : IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice) =>
        [ new("Giveaway", originalPrice) ];
}