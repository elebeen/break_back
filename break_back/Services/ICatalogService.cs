using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Dtos.Restaurant;

namespace break_back.Services;

public interface ICatalogService
{
    Task<IEnumerable<RestaurantDto>> GetActiveRestaurants();
    Task<IEnumerable<MealDto>> GetMenuByRestaurant(Guid restaurantId);
    Task<MealDetailsDto?> GetMealDetails(Guid mealId);
}