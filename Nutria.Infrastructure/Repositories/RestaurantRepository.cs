using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class RestaurantRepository: Repository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(AppdbContext context) : base(context)
    {
    }

    public async Task<List<Restaurant>> GetActiveRestaurantsAsync()
    {
        return await _context.Restaurants
            .Where(r => r.IsActive == true)
            .ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantWithMealsAsync(Guid restaurantId)
    {
        return await _context.Restaurants
            .Include(r => r.Meals)
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }
}