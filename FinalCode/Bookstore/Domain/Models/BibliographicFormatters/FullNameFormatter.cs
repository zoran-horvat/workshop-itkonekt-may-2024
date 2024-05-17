using System.Text;

namespace Bookstore.Domain.Models.BibliographicFormatters;

public class FullNameFormatter : IAuthorNameFormatter
{
    public Citation ToCitation(Person author) =>
        new BookAuthorSegment($"{author.FirstName} {author.LastName}", author.Id);
}