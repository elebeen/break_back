using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AppdbContext _context;

    public Repository(AppdbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
    
    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
    
    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public IQueryable<TEntity> Query()
    {
        return _context.Set<TEntity>().AsQueryable();
    }
}