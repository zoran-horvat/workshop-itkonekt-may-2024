using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;

namespace Bookstore.Data.Seeding.DataSeed;

public class AuthorsSeed : IDataSeed<Person>
{
    private readonly BookstoreDbContext _dbContext;

    public AuthorsSeed(BookstoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedAsync() => Task.CompletedTask;

    public async Task<Person> EnsureEqualExists(Person entity)
    {
        if (_dbContext.People.GetPeople().ByName(entity.FirstName, entity.LastName).FirstOrDefault() is Person author)
        {
            return author;
        }

        _dbContext.People.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}