using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public class BookPrice
{
    public Guid Id { get; private set; } = Guid.Empty;
    public DateTime ValidFrom { get; private set; } = DateTime.MinValue;
    public Guid BookId { get; private set; } = Guid.Empty;
    public Money Price
    { 
        get => this.Currency.Amount(this.Amount);
        private set => (this.Amount, this.Currency) = (value.Amount, value.Currency);
    }

    private decimal Amount { get; set; } = 0;       // Used by EF Core
    private Currency Currency { get; set; }         // Used by EF Core

    private BookPrice() { }  // Used by EF Core

    public static BookPrice For(Book book, Money price, DateTime validFrom) => For(book.Id, price, validFrom);

    public static BookPrice For(Guid bookId, Money price, DateTime validFrom) =>
        new()
        {
            Id = Guid.NewGuid(),
            BookId = bookId,
            Price = price,
            ValidFrom = validFrom
        };
}