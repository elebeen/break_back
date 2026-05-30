namespace Nutria.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> FindAll();
    TEntity FindById(int id);
    TEntity FindbyGuid(Guid guid);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    TEntity FindByName(string name);
    IQueryable<TEntity> GetAllQueryable();
}