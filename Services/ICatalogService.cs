using break_back.Models.Dtos.RestaurantDtos;
using break_back.Models.Dtos.MealDtos;

namespace break_back.Services;

public interface ICatalogService
{
    Task<IEnumerable<RestaurantDto>> GetActiveRestaurants();
    Task<IEnumerable<MealDto>> GetMenuByRestaurant(Guid restaurantId);
    Task<MealDetailsDto?> GetMealDetails(Guid mealId);
}