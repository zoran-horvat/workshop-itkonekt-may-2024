namespace Bookstore.Domain.Models;

public interface IBibliographicEntryFormatter
{
    Citation ToCitation(Book book);
}