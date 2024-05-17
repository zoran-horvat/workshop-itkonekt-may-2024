namespace Bookstore.Data.Seeding;

public class NoSeed<TEntity> : IDataSeed<TEntity>
{
    public Task SeedAsync() => Task.CompletedTask;
    public Task<TEntity> EnsureEqualExists(TEntity entity) => Task.FromResult(entity);
}