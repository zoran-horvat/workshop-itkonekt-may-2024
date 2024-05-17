using System.Linq.Expressions;

namespace Bookstore.Data;

public interface ISpecification<TEntity> where TEntity : class
{
    ISpecification<TEntity> And(Expression<Func<TEntity, bool>> condition);
    ISpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector);
    ISpecification<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector);
}