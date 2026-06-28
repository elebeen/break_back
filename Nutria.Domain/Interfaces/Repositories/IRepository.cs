using System.Linq.Expressions;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);
    
    void Update(TEntity entity);

    void Delete(TEntity entity);

    IQueryable<TEntity> Query();
}