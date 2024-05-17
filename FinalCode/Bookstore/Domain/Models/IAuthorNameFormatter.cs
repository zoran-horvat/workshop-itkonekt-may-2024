namespace Bookstore.Domain.Models;

public interface IAuthorNameFormatter
{
    Citation ToCitation(Person author);
}