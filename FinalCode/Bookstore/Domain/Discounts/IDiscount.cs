using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public interface IDiscount
{
    IEnumerable<DiscountApplication> ApplyTo(Money originalPrice);

    public static IDiscount NoDiscount => new NoDiscount();
    public static IDiscount GiveForFree => new GiveForFree();
}

public record DiscountApplication(string Label, Money DiscountedAmount);
