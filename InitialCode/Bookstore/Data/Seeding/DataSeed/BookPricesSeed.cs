using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding;

public class BookPricesSeed : IDataSeed<BookPrice>
{
    private readonly BookstoreDbContext _dbContext;
    private DateTime Timestamp { get; } = DateTime.UtcNow.AddDays(-100);

    public BookPricesSeed(BookstoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BookPrice> EnsureEqualExists(BookPrice entity)
    {
        if (_dbContext.BookPricesDbSet.ForBook(entity.BookId).FirstOrDefault() is BookPrice existing) return existing;

        _dbContext.BookPricesDbSet.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task SeedAsync()
    {
        if (await _dbContext.BookPricesDbSet.AnyAsync()) return;

        IEnumerable<Book> books = await _dbContext.BooksDbSet.ToListAsync();
        foreach (Book book in books)
        {
            await EnsureEqualExists(this.PriceFor(book));
        }
    }

    private BookPrice PriceFor(Book book)
    {
        Random rand = new(book.Title.GetStableHashCode());
        return BookPrice.For(book, new Money(rand.Next(2500, 4800) / 100M, Currency.USD), this.Timestamp);
    }
}