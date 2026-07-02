using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IMealRepository
{
    Task<List<MealDto>> GetMealsByRestaurantAsync(Guid restaurantId);
    Task<Meal?> GetMealWithNutritionalInfoAsync(Guid mealId);
    Task<List<Meal>> GetMealsByCaloriesAsync(int maxCalories);
    Task<List<Meal>> GetMealsByConditionAsync(string conditionType);
    Task<MealDetailsDto?> GetMealDetails(Guid mealId);
    Task<List<Meal>> GetMealsByIdsAsync(List<Guid> mealIds);
    Task<List<MealWithIndicatorsDto>> GetMealsByUserId(Guid userId);
    Task<List<Meal>> SearchMealsByNameAsync(string name);
    Task<List<MealDto>> GetAllMealsAsync();
}