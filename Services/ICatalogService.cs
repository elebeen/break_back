using break_back.Models.Dtos.RestaurantDtos;
using break_back.Models;

namespace break_back.Services;

public interface ICatalogService
{
    Task<IEnumerable<RestaurantDto>> GetActiveRestaurants();
    Task<IEnumerable<Meal>> GetMenuByRestaurant(Guid restaurantId);
    Task<Meal?> GetMealDetails(Guid mealId);
}