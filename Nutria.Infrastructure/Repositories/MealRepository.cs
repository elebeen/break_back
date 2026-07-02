using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Dtos.NutritionalInfo;
using Nutria.Domain.Interfaces.Repositories;
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
                    : conditionType == "Hypertension"
                        ? m.NutritionalInfo!.SodiumMg < 500
                        : true)
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
    
    public async Task<List<Meal>> GetMealsByIdsAsync(List<Guid> mealIds)
    {
        return await _context.Meals
            .Where(x => mealIds.Contains(x.Id))
            .ToListAsync();
    }
    
    public async Task<List<MealWithIndicatorsDto>> GetMealsByUserId(Guid userId)
    {
        var profile = await _context.HealthProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (profile is null)
            return [];
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.HealthProfile)
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
       var mealData = await _context.Meals
            .AsNoTracking()
            .Where(m => m.IsActive == true)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Price,
                m.RestaurantId,
                RestaurantName = m.Restaurant.Name,
                NutritionalInfo = m.NutritionalInfo != null ? new
                {
                    m.NutritionalInfo.Calories,
                    m.NutritionalInfo.SugarG,
                    m.NutritionalInfo.SodiumMg
                } : null,
                Allergens = m.Ingredients
                    .Where(i => i.IsAllergen)
                    .Select(i => i.Name.ToLower())
                    .ToList()
            }).ToListAsync();

        var result = new List<MealWithIndicatorsDto>(mealData.Count);
        var userConditions = user.Conditions.Select(c => c.Name.ToLower()).ToList();

        var dailyCalorieTarget = user.HealthProfile?.DailyCalorieTarget ?? 0;
        var dailySugarLimit = user.HealthProfile?.DailySugarLimitG ?? 0;
        var dailySodiumLimit = user.HealthProfile?.DailySodiumLimitMg ?? 0;

        foreach (var m in mealData)
        {
            var dto = new MealWithIndicatorsDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                RestaurantId = m.RestaurantId,
                RestaurantName = m.RestaurantName,
                SpecificWarnings = []
            };

            if (m.NutritionalInfo != null)
            {
                dto.ExceedsCalorieLimit = dailyCalorieTarget > 0 && m.NutritionalInfo.Calories > dailyCalorieTarget;
                dto.ExceedsSugarLimit = dailySugarLimit > 0 && m.NutritionalInfo.SugarG > dailySugarLimit;
                dto.ExceedsSodiumLimit = dailySodiumLimit > 0 && m.NutritionalInfo.SodiumMg > dailySodiumLimit;
            }

            foreach (var condition in userConditions
                         .Where(condition => m.Allergens
                             .Any(a => condition
                                 .Contains(a) || a.Contains(condition))))
            {
                dto.HasAllergenWarning = true;
                dto.SpecificWarnings.Add($"Contiene ingredientes relacionados con: {condition}");
            }
            result.Add(dto);
        }

        return result;
    }
    
    public async Task<List<Meal>> SearchMealsByNameAsync(string name)
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }
    
    public async Task<List<MealDto>> GetAllMealsAsync()
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(m => m.IsActive == true)
            .Select(m => new MealDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive,

                NutritionalInfo = m.NutritionalInfo == null
                    ? null
                    : new NutritionalInfoDto
                    {
                        Calories = m.NutritionalInfo.Calories,
                        ProteinG = m.NutritionalInfo.ProteinG,
                        CarbsG = m.NutritionalInfo.CarbsG,
                        FatsG = m.NutritionalInfo.FatsG,
                        SodiumMg = m.NutritionalInfo.SodiumMg,
                        SugarG = m.NutritionalInfo.SugarG,
                        FiberG = m.NutritionalInfo.FiberG
                    }

            })
            .ToListAsync();
    }
}