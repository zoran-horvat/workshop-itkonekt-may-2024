using System.Linq.Expressions;
using Bookstore.Domain.Models;

namespace Bookstore.Data.Specifications;

public static class BookSpecs
{
    public static Expression<Func<Book, bool>> ById(Guid id) => book => book.Id == id;

    public static Expression<Func<Book, bool>> ByAuthorInitial(string initial) =>
        book => book.AuthorsCollection.Any(author => author.Person.LastName.StartsWith(initial));

    public static Expression<Func<Book, object>> Title => book => book.Title;

    public static ISpecification<Book> ById(this ISpecification<Book> spec, Guid id) => spec.And(ById(id));
    public static ISpecification<Book> ByAuthorInitial(this ISpecification<Book> spec, string initial) => spec.And(ByAuthorInitial(initial));
    public static ISpecification<Book> OrderByTitle(this ISpecification<Book> spec) => spec.OrderBy(Title);
}