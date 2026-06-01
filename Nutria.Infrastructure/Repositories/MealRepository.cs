using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class MealRepository : Repository<Meal>, IMealRepository
{
    public MealRepository(AppdbContext context) : base(context)
    {
    }

    public async Task<List<Meal>> GetMealsByRestaurantAsync(Guid restaurantId)
    {
        return await _context.Meals
            .Where(m => m.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<Meal?> GetMealWithNutritionalInfoAsync(Guid mealId)
    {
        return await _context.Meals
            .Include(m => m.NutritionalInfo)
            .Include(m => m.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == mealId);
    }

    public async Task<List<Meal>> GetMealsByCaloriesAsync(int maxCalories)
    {
        return await _context.Meals
            .Include(m => m.NutritionalInfo)
            .Where(m => m.NutritionalInfo != null &&
                        m.NutritionalInfo.Calories <= maxCalories)
            .ToListAsync();
    }

    public async Task<List<Meal>> GetMealsByConditionAsync(string conditionType)
    {
        return await _context.Meals
            .Include(m => m.NutritionalInfo)
            .Where(m =>
                conditionType == "Diabetes"
                    ? m.NutritionalInfo!.SugarG < 10
                    : conditionType == "Hypertension"
                        ? m.NutritionalInfo!.SodiumMg < 500
                        : true)
            .ToListAsync();
    }
    
    public async Task<List<Meal>> GetMealsByIdsAsync(List<Guid> mealIds)
    {
        return await _context.Meals
            .Where(x => mealIds.Contains(x.Id))
            .ToListAsync();
    }
    
    public async Task<List<Meal>> GetCompatibleMealsAsync(Guid userId)
    {
        var profile = await _context.HealthProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (profile is null)
            return new List<Meal>();

        return await _context.Meals
            .AsNoTracking()
            .Include(x => x.NutritionalInfo)
            .Where(x =>
                x.NutritionalInfo.Calories <= profile.DailyCalorieTarget)
            .ToListAsync();
    }
    
}