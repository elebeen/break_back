namespace Nutria.Domain.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChanges();
    
    IHealthRepository HealthRepository { get; set; }
}