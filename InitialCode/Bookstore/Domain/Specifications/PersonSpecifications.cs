using Bookstore.Domain.Models;

namespace Bookstore.Domain.Specifications;

public static class PersonSpecifications
{
    public static IQueryable<Person> GetPeople(this IQueryable<Person> authors) => authors;

    public static IQueryable<Person> ByName(this IQueryable<Person> authors, string firstName, string lastName) =>
        authors.Where(author => author.FirstName == firstName && author.LastName == lastName);

    public static IQueryable<Person> GetPublishedAuthors(this IQueryable<BookAuthor> bookAuthors) =>
        bookAuthors.Select(bookAuthor => bookAuthor.Person).Distinct();

}