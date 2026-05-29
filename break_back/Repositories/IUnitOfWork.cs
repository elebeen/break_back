namespace break_back.Repositories;

public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChanges();
    
    IHealthRepository HealthRepository { get; set; }
}