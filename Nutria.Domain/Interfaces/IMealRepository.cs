using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IMealRepository
{
    Task<List<Meal>> GetMealsByRestaurantAsync(Guid restaurantId);

    Task<Meal?> GetMealWithNutritionalInfoAsync(Guid mealId);

    Task<List<Meal>> GetMealsByCaloriesAsync(int maxCalories);

    Task<List<Meal>> GetMealsByConditionAsync(string conditionType);
}