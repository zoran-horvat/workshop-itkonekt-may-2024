using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Implementation;

public class DbSetRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IQueryable<TEntity> _baseQuery;

    public DbSetRepository(DbSet<TEntity> dbSet, IQueryable<TEntity> baseQuery) =>
        (_dbSet, _baseQuery) = (dbSet, baseQuery);

    public IQueryable<TEntity> All => _baseQuery;

    public void Add(TEntity entity) => _dbSet.Add(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public async Task<List<TEntity>> QueryAsync(ISpecification<TEntity> specification) =>
        await Apply(specification).ToListAsync();

    public async Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity> specification) =>
        await Apply(specification).SingleOrDefaultAsync();

    private IQueryable<TEntity> Apply(ISpecification<TEntity> specification) =>
        specification is QueryableSpecification<TEntity> queryableSpecification ? Apply(queryableSpecification)
        : throw new ArgumentException("Specification is not of expected implementation type", nameof(specification));

    private IQueryable<TEntity> Apply(QueryableSpecification<TEntity> specification)
    {
        IQueryable<TEntity> filteredQuery = specification.Conditions
            .Aggregate(_baseQuery, (query, condition) => query.Where(condition));

        using IEnumerator<(Expression<Func<TEntity, object>> keySelector, bool isAscending)> orderByEnumerator =
            specification.OrderByExpressions.GetEnumerator();

        if (!orderByEnumerator.MoveNext()) return filteredQuery;

        IOrderedQueryable<TEntity> orderedQuery = orderByEnumerator.Current.isAscending
            ? filteredQuery.OrderBy(orderByEnumerator.Current.keySelector)
            : filteredQuery.OrderByDescending(orderByEnumerator.Current.keySelector);

        while (orderByEnumerator.MoveNext())
        {
            orderedQuery = orderByEnumerator.Current.isAscending
                ? orderedQuery.ThenBy(orderByEnumerator.Current.keySelector)
                : orderedQuery.ThenByDescending(orderByEnumerator.Current.keySelector);
        }

        return orderedQuery;
    }
}
