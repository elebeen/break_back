using break_back.Models;
using break_back.Models.Dtos.RestaurantDtos;
using break_back.Models.Dtos.MealDtos;
using break_back.Models.Dtos.NutritionalInfo;
using break_back.Models.Dtos.Ingredients;
using Microsoft.EntityFrameworkCore;

namespace break_back.Services.Implements;

public class CatalogService : ICatalogService
{
    private readonly Context _context;

    public CatalogService(Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RestaurantDto>> GetActiveRestaurants()
    {
        return await _context.Restaurants
            .AsNoTracking()                    // Mejor performance (recomendado en consultas de solo lectura)
            .Where(r => r.IsActive == true)
            .Select(r => new RestaurantDto
            {
                Name         = r.Name,
                Address      = r.Address,
                ContactPhone = r.ContactPhone,
                IsActive     = r.IsActive
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MealDto>> GetMenuByRestaurant(Guid restaurantId)
    {
        return await _context.Meals
            .AsNoTracking()                    // Mejor rendimiento
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
            })
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

                Ingredients = m.Ingredients
                    .Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        IsAllergen = i.IsAllergen
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}