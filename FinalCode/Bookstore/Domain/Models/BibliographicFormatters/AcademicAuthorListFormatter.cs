namespace Bookstore.Domain.Models.BibliographicFormatters;

public class AcademicAuthorListFormatter : IAuthorListFormatter
{
    private IAuthorNameFormatter NameFormatter { get; } = new AcademicNameFormatter();

    public Citation ToCitation(IEnumerable<Person> authors) =>
        FormatToCitations(authors.ToArray());

    private Citation FormatToCitations(Person[] authors) => authors switch
    {
        [] => Citation.Empty,
        [var author] => NameFormatter.ToCitation(author),
        [var author1, var author2] => NameFormatter.ToCitation(author1).Add(", ").Add(NameFormatter.ToCitation(author2)),
        [var author0, ..] => NameFormatter.ToCitation(author0).Add(", et al.")
    };
}