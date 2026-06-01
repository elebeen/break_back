using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Dtos.NutritionalInfo;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class MealRepository : Repository<Meal>, IMealRepository
{
    public MealRepository(AppdbContext context) : base(context) { }

    public async Task<List<MealDto>> GetMealsByRestaurantAsync(Guid restaurantId)
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(m => m.RestaurantId == restaurantId && m.IsActive == true)
            .Select(m => new MealDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive,
                NutritionalInfo = m.NutritionalInfo != null ? new NutritionalInfoDto
                {
                    Calories = m.NutritionalInfo.Calories,
                    ProteinG = m.NutritionalInfo.ProteinG,
                    CarbsG = m.NutritionalInfo.CarbsG,
                    FatsG = m.NutritionalInfo.FatsG,
                    SodiumMg = m.NutritionalInfo.SodiumMg,
                    SugarG = m.NutritionalInfo.SugarG,
                    FiberG = m.NutritionalInfo.FiberG
                } : null
            }).ToListAsync();
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
                    : conditionType != "Hypertension" || m.NutritionalInfo!.SodiumMg < 500)
            .ToListAsync();
    }

    public async Task<MealDetailsDto?> GetMealDetails(Guid mealId)
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(m => m.Id == mealId)
            .Select(m => new MealDetailsDto
            {
                Id = m.Id,
                RestaurantId = m.RestaurantId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive,
                NutritionalInfo = m.NutritionalInfo != null ? new NutritionalInfoDto
                {
                    Calories = m.NutritionalInfo.Calories,
                    ProteinG = m.NutritionalInfo.ProteinG,
                    CarbsG = m.NutritionalInfo.CarbsG,
                    FatsG = m.NutritionalInfo.FatsG,
                    SodiumMg = m.NutritionalInfo.SodiumMg,
                    SugarG = m.NutritionalInfo.SugarG,
                    FiberG = m.NutritionalInfo.FiberG
                } : null,
                Ingredients = m.Ingredients.Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsAllergen = i.IsAllergen
                }).ToList()
            }).FirstOrDefaultAsync();
    }
}