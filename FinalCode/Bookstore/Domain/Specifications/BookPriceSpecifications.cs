using Bookstore.Domain.Models;

namespace Bookstore.Domain.Specifications;

public static class BookPriceSpecifications
{
    public static IQueryable<BookPrice> ForBook(this IQueryable<BookPrice> bookPrices, Guid bookId) => bookPrices
        .Where(price => price.BookId == bookId);

    public static IQueryable<BookPrice> For(this IQueryable<BookPrice> bookPrices, Book book) =>
        bookPrices.ForBook(book.Id);

    public static IQueryable<BookPrice> At(this IQueryable<BookPrice> bookPrices, DateTime time) => bookPrices
        .Where(price => price.ValidFrom <= time)
        .GroupBy(price => price.BookId)
        .Select(group => group.OrderByDescending(price => price.ValidFrom).First());
}