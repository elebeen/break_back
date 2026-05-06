using System.Linq.Expressions;

namespace break_back.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> FindAll();
    TEntity FindById(int id);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    TEntity FindByName(string name);
    IQueryable<TEntity> GetAllQueryable();
}