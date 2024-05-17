namespace Bookstore.Domain.Models;

public class Book
{
    public Guid Id { get; private set; } = Guid.Empty;
    public string Title { get; private set; } = string.Empty;
    internal ICollection<BookAuthor> AuthorsCollection { get; set; } = new List<BookAuthor>();
    public IEnumerable<Person> Authors => AuthorsCollection.Select(author => author.Person);

    private Book() { }  // Used by EF Core

    public static Book CreateNew(string title, params Person[] authors)
    {
        Book book = new()
        {
            Title = title,
            Id = Guid.NewGuid()
        };
        book.AuthorsCollection = BookAuthor.CreateMany(book, authors).ToList();
        return book;
    }
}
