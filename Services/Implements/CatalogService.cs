using break_back.Models;
using Microsoft.EntityFrameworkCore;

namespace break_back.Services.Implements;

public class CatalogService : ICatalogService
{
    private readonly Context _context;

    public CatalogService(Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Restaurant>> GetActiveRestaurants()
    {
        // Filtramos solo los restaurantes marcados como activos
        return await _context.Restaurants
            .Where(r => r.IsActive == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Meal>> GetMenuByRestaurant(Guid restaurantId)
    {
        // Obtenemos las comidas de un restaurante incluyendo su info nutricional
        return await _context.Meals
            .Include(m => m.NutritionalInfo)
            .Where(m => m.RestaurantId == restaurantId && m.IsActive == true)
            .ToListAsync();
    }

    public async Task<Meal?> GetMealDetails(Guid mealId)
    {
        // Carga completa: Comida + Info Nutricional + Ingredientes
        return await _context.Meals
            .Include(m => m.NutritionalInfo)
            .Include(m => m.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == mealId);
    }
}