namespace Nutria.Domain.Interfaces.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IMealRepository Meals { get; }

    IOrderRepository Orders { get; }

    IRestaurantRepository Restaurants { get; }

    IHealthRepository Health { get; }

    IMedicalConditionRepository MedicalConditions { get; }

    IRepository<TEntity> Repository<TEntity>() where TEntity : class;

    Task<int> SaveChanges();
    
}