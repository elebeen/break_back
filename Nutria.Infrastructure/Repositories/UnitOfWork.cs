using System.Collections;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Hashtable _repositories;
    private readonly AppdbContext _appdbContext;
    
    public IUserRepository Users { get; }

    public IMealRepository Meals { get; }

    public IOrderRepository Orders { get; }

    public IRestaurantRepository Restaurants { get; }

    public IHealthRepository Health { get; }

    public IMedicalConditionRepository MedicalConditions { get; }
    
    public UnitOfWork(
        AppdbContext appdbContext,
        IUserRepository userRepository,
        IMealRepository mealRepository,
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IHealthRepository healthRepository,
        IMedicalConditionRepository medicalConditionRepository)
    {
        _appdbContext = appdbContext;

        _repositories = new Hashtable();

        Users = userRepository;

        Meals = mealRepository;

        Orders = orderRepository;

        Restaurants = restaurantRepository;

        Health = healthRepository;

        MedicalConditions = medicalConditionRepository;
    }

    public Task<int> SaveChanges()
    {
        return _appdbContext.SaveChangesAsync();
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;
        
        if (_repositories.ContainsKey(type))
        {
            return (IRepository<TEntity>)_repositories[type];
        }

        var repoType = typeof(Repository<>);
        var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _appdbContext);

        if (repoInstance != null)
        {
            _repositories.Add(type, repoInstance);
            return (IRepository<TEntity>)repoInstance;
        }
        
        throw new Exception($"Repository {type} can not be created.");
    }
}