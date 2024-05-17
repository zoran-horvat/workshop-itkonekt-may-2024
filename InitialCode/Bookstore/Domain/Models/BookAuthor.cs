namespace Bookstore.Domain.Models;

public class BookAuthor
{
    public int Ordinal { get; private set; } = 0;
    public Person Person { get; private set; } = null!;
    public Book Book { get; private set; } = null!;

    private BookAuthor() { }  // Used by EF Core

    public static IEnumerable<BookAuthor> CreateMany(Book book, IEnumerable<Person> authors) =>
        authors.Select((person, index) => new BookAuthor()
        {
            Book = book,
            Ordinal = index + 1,
            Person = person
        });
}