namespace Bookstore.Domain.Models.BibliographicFormatters;

public class TitleOnlyFormatter : IBibliographicEntryFormatter
{
    public string Format(Book book) => book.Title;
    public Citation ToCitation(Book book) => Citation.Empty.Add(new BookTitleSegment(book.Title, book.Id));
}