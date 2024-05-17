namespace Bookstore.Domain.Models.BibliographicFormatters;

public class ShortNameFormatter : IAuthorNameFormatter
{
    public Citation ToCitation(Person author) =>
        new BookAuthorSegment($"{author.FirstName[..1]}. {author.LastName}", author.Id);
}
