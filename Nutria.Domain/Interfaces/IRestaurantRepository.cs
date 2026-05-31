using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IRestaurantRepository
{
    Task<List<Restaurant>> GetActiveRestaurantsAsync();

    Task<Restaurant?> GetRestaurantWithMealsAsync(Guid restaurantId);
}