namespace Bookstore.Domain.Discounts.Implementation;

using System.Collections.Generic;
using Bookstore.Domain.Common;

public class SerialDiscounts : IDiscount
{
    private readonly IDiscount[] _discounts;

    private SerialDiscounts(params IDiscount[] discounts) =>
        _discounts = discounts;

    public static IDiscount Of(params IDiscount[] discounts) =>
        discounts.Where(d => !(d is NoDiscount)).ToArray() switch
        {
            [] => new NoDiscount(),
            [var single] => single,
            var many => new SerialDiscounts(many),
        };

    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice)
    {
        Money remaining = originalPrice;
        foreach (var application in _discounts.SelectMany(d => d.ApplyTo(remaining)))
        {
            yield return application;
            remaining -= application.DiscountedAmount;
        }
    }
}