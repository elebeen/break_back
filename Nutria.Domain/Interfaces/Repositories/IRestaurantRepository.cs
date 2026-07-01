using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IRestaurantRepository
{
    Task<List<RestaurantDto>> GetActiveRestaurantsAsync();

    Task<Restaurant?> GetRestaurantWithMealsAsync(Guid restaurantId);
    Task<RestaurantDto> GetRestaurantByIdAsync(Guid restaurantId);
}