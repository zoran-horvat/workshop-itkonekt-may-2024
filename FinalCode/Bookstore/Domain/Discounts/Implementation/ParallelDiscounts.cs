namespace Bookstore.Domain.Discounts.Implementation;

using System.Collections.Generic;
using Bookstore.Domain.Common;

delegate IDiscount Wrap(params IDiscount[] discounts);

public sealed class ParallelDiscounts : IDiscount
{
    private readonly IDiscount[] _discounts;

    private ParallelDiscounts(params IDiscount[] discounts) =>
        _discounts = discounts;

    public static IDiscount Of(params IDiscount[] discounts) =>
        discounts.Where(d => !(d is NoDiscount)).ToArray() switch
        {
            [] => new NoDiscount(),
            [var single] => single,
            var many => Cap.ToPrice(new ParallelDiscounts(many)),
        };

    public IEnumerable<DiscountApplication> ApplyTo(Money originalPrice) =>
        _discounts.SelectMany(d => d.ApplyTo(originalPrice));
}
