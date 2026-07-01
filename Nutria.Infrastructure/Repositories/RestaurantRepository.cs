using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class RestaurantRepository(AppdbContext context)
    : Repository<Restaurant>(context), IRestaurantRepository
{
    public async Task<List<RestaurantDto>> GetActiveRestaurantsAsync()
    {
        return await _context.Restaurants
            .AsNoTracking()
            .Where(r => r.IsActive == true)
            .Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                ContactPhone = r.ContactPhone,
                IsActive = r.IsActive
            })
            .ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantWithMealsAsync(Guid restaurantId)
    {
        return await _context.Restaurants
            .Include(r => r.Meals)
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }
}