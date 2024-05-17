namespace Bookstore.Domain.Models.BibliographicFormatters;

public class AcademicNameFormatter : IAuthorNameFormatter
{
    public Citation ToCitation(Person author) =>
        new BookAuthorSegment($"{author.LastName}, {author.FirstName[..1]}.", author.Id);
}