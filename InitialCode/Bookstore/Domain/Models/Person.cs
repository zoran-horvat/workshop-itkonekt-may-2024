namespace Bookstore.Domain.Models;

public class Person
{
    public Guid Id { get; private set; } = Guid.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private Person() { }  // Used by EF Core

    public static Person CreateNew(string firstName, string lastName) => new()
    {
        FirstName = firstName,
        LastName = lastName
    };

    public static Person CreateNew(string firstName) => new()
    {
        FirstName = firstName,
        Id = Guid.NewGuid()
    };
}