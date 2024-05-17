namespace Bookstore.Data.Seeding;

public interface IDataSeed<TEntity>
{
    Task SeedAsync();
    Task<TEntity> EnsureEqualExists(TEntity entity);

    async Task<IEnumerable<TEntity>> EnsureEqualExists(IEnumerable<TEntity> entities)
    {
        List<TEntity> result = new();
        foreach (TEntity entity in entities)
            result.Add(await EnsureEqualExists(entity));
        return result;
    }
}