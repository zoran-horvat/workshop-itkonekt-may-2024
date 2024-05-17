using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding.DataSeed;

public class BooksSeed : IDataSeed<Book>
{
    private readonly BookstoreDbContext _dbContext;
    private readonly IDataSeed<Person> _authorsSeed;

    public BooksSeed(BookstoreDbContext dbContext, IDataSeed<Person> authorsSeed)
    {
        _dbContext = dbContext;
        _authorsSeed = authorsSeed;
    }

    public async Task SeedAsync()
    {
        if (_dbContext.BooksDbSet.Any()) return;

        foreach ((string title, (string firstName, string lastName)[] authors) bookData in BooksData)
        {
            Person[] authors = (await _authorsSeed.EnsureEqualExists(bookData.authors.Select(ToPerson))).ToArray();
            Book book = Book.CreateNew(bookData.title, authors);
            await EnsureEqualExists(book);
        }
    }

    private Person ToPerson((string firstName, string lastName) person) =>
        Person.CreateNew(person.firstName, person.lastName);

    public async Task<Book> EnsureEqualExists(Book entity)
    {
        if (_dbContext.Books.All.Where(book => book.Title == entity.Title).FirstOrDefault() is Book book)
        {
            return book;
        }
        _dbContext.BooksDbSet.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    private (string title, (string firstName, string lastName)[] authors)[] BooksData => new (string, (string, string)[])[]
    {
        ("Clean Code: A Handbook of Agile Software Craftsmanship", new (string, string)[] { ("Robert", "Martin") }),
        ("The Pragmatic Programmer: From Journeyman to Master", new (string, string)[] { ("Andrew", "Hunt"), ("David", "Thomas") }),
        ("Design Patterns: Elements of Reusable Object-Oriented Software", new (string, string)[] { ("Erich", "Gamma"), ("Richard", "Helm"), ("Ralph", "Johnson"), ("John", "Vlissides") }),
        ("The C Programming Language", new (string, string)[] { ("Brian", "Kernighan"), ("Dennis", "Ritchie") }),
        ("Code Complete: A Practical Handbook of Software Construction", new (string, string)[] { ("Steve", "McConnell") }),
        ("Refactoring: Improving the Design of Existing Code", new (string, string)[] { ("Martin", "Fowler") }),
        ("Cracking the Coding Interview: 189 Programming Questions and Solutions", new (string, string)[] { ("Gayle", "Laakmann McDowell") }),
        ("The Clean Architecture: A Craftsman's Guide to Software Structure and Design", new (string, string)[] { ("Robert", "Martin") }),
        ("Effective Java", new (string, string)[] { ("Joshua", "Bloch") }),
        ("Programming Pearls", new (string, string)[] { ("Jon", "Bentley") }),
        ("Test-Driven Development: By Example", new (string, string)[] { ("Kent", "Beck") }),
        ("Object-Oriented Software Construction", new (string, string)[] { ("Bertrand", "Meyer") }),
        ("Domain-Driven Design: Tackling Complexity in the Heart of Software", new (string, string)[] { ("Eric", "Evans") }),
        ("Patterns of Enterprise Application Architecture", new (string, string)[] { ("Martin", "Fowler") }),
        ("The Mythical Man-Month: Essays on Software Engineering", new (string, string)[] { ("Frederick", "Brooks") }),
        ("Code: The Hidden Language of Computer Hardware and Software", new (string, string)[] { ("Charles", "Petzold") }),
        ("The Art of Computer Programming, Volumes 1-4A Boxed Set", new (string, string)[] { ("Donald", "Knuth") }),
        ("Working Effectively with Legacy Code", new (string, string)[] { ("Michael", "Feathers") })
    };
}