namespace Bookstore.Domain.Models;

public interface IAuthorListFormatter
{
    Citation ToCitation(IEnumerable<Person> authors);
}