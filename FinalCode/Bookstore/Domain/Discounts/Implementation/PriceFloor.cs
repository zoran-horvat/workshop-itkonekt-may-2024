using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public sealed class PriceFloor(Money floor, IDiscount discount, IDiscount next) : IDiscount
{
    private Money _floor = floor;
    private IDiscount _discount = discount;
    private IDiscount _next = next;

    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice) => 
        originalPrice.CompareTo(_floor) >= 0
            ? _discount.ApplyTo(originalPrice)
            : _next.ApplyTo(originalPrice);
}