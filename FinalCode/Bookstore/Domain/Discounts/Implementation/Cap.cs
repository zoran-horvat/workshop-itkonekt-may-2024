using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public abstract class Cap(IDiscount other) : IDiscount
{
    private readonly IDiscount _discount = other;

    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice)
    {
        var remaining = GetCapAmount(originalPrice);
        foreach (var application in _discount.ApplyTo(originalPrice))
        {
            if (remaining.CompareTo(application.DiscountedAmount) >= 0)
            {
                yield return application;
                remaining -= application.DiscountedAmount;
            }
            else
            {
                yield return new DiscountApplication(application.Label + " (capped)", remaining);
                break;
            }
        }
    }

    protected abstract Money GetCapAmount(Money originalPrice);

    public static IDiscount Absolute(IDiscount other, Money amount) => new AbsoluteCap(other, amount);

    public static IDiscount Relative(IDiscount other, decimal factor) => new RelativeCap(other, factor);

    public static IDiscount ToPrice(IDiscount other) => Relative(other, 1);
}
