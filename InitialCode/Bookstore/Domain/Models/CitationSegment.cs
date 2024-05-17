namespace Bookstore.Domain.Models;

public abstract record CitationSegment(string Text)
{
    public static implicit operator CitationSegment(string text) => new TextSegment(text);
    public static implicit operator CitationSegment(Book book) => new BookTitleSegment(book.Title, book.Id);
}

public sealed record BookAuthorSegment(string FormattedName, Guid AuthorId) : CitationSegment(FormattedName);
public sealed record BookTitleSegment(string Title, Guid BookId) : CitationSegment(Title);
public sealed record TextSegment(string Text) : CitationSegment(Text);
