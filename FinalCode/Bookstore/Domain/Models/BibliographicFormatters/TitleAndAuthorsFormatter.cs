namespace Bookstore.Domain.Models.BibliographicFormatters;

public class TitleAndAuthorsFormatter : IBibliographicEntryFormatter
{
    private readonly IAuthorListFormatter _authorListFormatter;
    private readonly string _separator;
    private delegate (Citation a, Citation b) JoinStrategy(Citation title, Citation authors);
    private readonly JoinStrategy _joinStrategy;

    private TitleAndAuthorsFormatter(IAuthorListFormatter authorListFormatter, JoinStrategy joinStrategy, string separator = ", ") =>
        (_authorListFormatter, _joinStrategy, _separator) = (authorListFormatter, joinStrategy, separator);

    public static IBibliographicEntryFormatter TitleThenAuthors(IAuthorListFormatter authorListFormatter, string separator = ", ") =>
        new TitleAndAuthorsFormatter(authorListFormatter, (t, a) => (t, a), separator);

    public static IBibliographicEntryFormatter AuthorsThenTitle(IAuthorListFormatter authorListFormatter, string separator = ", ") =>
        new TitleAndAuthorsFormatter(authorListFormatter, (t, a) => (a, t), separator);

    public static IBibliographicEntryFormatter Academic() =>
        AuthorsThenTitle(new AcademicAuthorListFormatter(), ", ");

    public Citation ToCitation(Book book)
    {
        Citation title = new BookTitleSegment(book.Title, book.Id);
        Citation authors = _authorListFormatter.ToCitation(book.Authors);
        (Citation a, Citation b) = _joinStrategy(title, authors);
        if (a.IsEmpty) return b;
        if (b.IsEmpty) return a;
        return a.Add(_separator).Add(b);
    }
}
