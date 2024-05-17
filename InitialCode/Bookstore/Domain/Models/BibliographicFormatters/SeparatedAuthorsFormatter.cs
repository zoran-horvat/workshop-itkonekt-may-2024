namespace Bookstore.Domain.Models.BibliographicFormatters;

public class SeparatedAuthorsFormatter : IAuthorListFormatter
{
    private readonly string _separator;
    private readonly string _lastSeparator;
    private readonly IAuthorNameFormatter _authorNameFormatter;

    public SeparatedAuthorsFormatter(IAuthorNameFormatter singleAuthorFormatter, string separator = ", ", string lastSeparator = " and ") =>
        (_authorNameFormatter, _separator, _lastSeparator) = (singleAuthorFormatter, separator, lastSeparator);

    public Citation ToCitation(IEnumerable<Person> authors) =>
        FormatToSegments(authors.Select(_authorNameFormatter.ToCitation).ToArray());

    private Citation FormatToSegments(Citation[] authors) => authors switch
    {
        [] => Citation.Empty,
        [var author] => author,
        [var author1, var author2] => author1.Add(_lastSeparator).Add(author2),
        [.. var first, var last] => Citation.Join(_separator, first).Add(_lastSeparator).Add(last)
    };
}