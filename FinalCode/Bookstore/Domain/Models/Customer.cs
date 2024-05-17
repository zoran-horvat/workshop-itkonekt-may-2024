namespace Bookstore.Domain.Models;

public class Customer
{
    public Guid Id { get; private set; } = Guid.Empty;
    public string Label { get; private set; } = string.Empty;

    private Customer() { }      // Used by EF Core

    public static Customer CreateNew(string label) =>
        new()
        {
            Id = Guid.NewGuid(),
            Label = label
        };
}