using break_back.Models.Dtos.Meal;
using break_back.Models;
using Microsoft.EntityFrameworkCore;

namespace break_back.Services.Implements;

public class RecommendationService : IRecommendationService

{
    private readonly Context _context;

    public RecommendationService(Context context)
    {
        _context = context;
    }

    public async Task<List<MealWithIndicatorsDto>> GetAnalyzedMenu(Guid userId)
    {
        // 1. Obtener perfil del usuario y sus condiciones médicas
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.HealthProfile)
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new List<MealWithIndicatorsDto>();
        
        // 2. Obtener platillos con su info nutricional e ingredientes
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
            })
            .ToListAsync();

        var result = new List<MealWithIndicatorsDto>(mealData.Count);

        var userConditions = user.Conditions
            .Select(c => c.Name.ToLower())
            .ToList();

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
                SpecificWarnings = new List<string>()
            };

            // Validaciones Nutricionales
            if (m.NutritionalInfo != null)
            {
                dto.ExceedsCalorieLimit = dailyCalorieTarget > 0 && 
                                          m.NutritionalInfo.Calories > dailyCalorieTarget;

                dto.ExceedsSugarLimit = dailySugarLimit > 0 && 
                                        m.NutritionalInfo.SugarG > dailySugarLimit;

                dto.ExceedsSodiumLimit = dailySodiumLimit > 0 && 
                                         m.NutritionalInfo.SodiumMg > dailySodiumLimit;
            }

            // Detección de Alérgenos
            foreach (var condition in userConditions)
            {
                if (m.Allergens.Any(a => condition.Contains(a) || a.Contains(condition)))
                {
                    dto.HasAllergenWarning = true;
                    dto.SpecificWarnings.Add($"Contiene ingredientes relacionados con: {condition}");
                }
            }

            result.Add(dto);
        }

        return result;
    }
}