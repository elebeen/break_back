using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class RestaurantRepository: Repository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(AppdbContext context) : base(context) { }

    public async Task<List<RestaurantDto>> GetActiveRestaurantsAsync()
    {
        return await _context.Restaurants
            .AsNoTracking()
            .Where(r => r.IsActive == true)
            .Select(r => new RestaurantDto
            {
                Name = r.Name,
                Address = r.Address,
                ContactPhone = r.ContactPhone,
                IsActive = r.IsActive
            }).ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantWithMealsAsync(Guid restaurantId)
    {
        return await _context.Restaurants
            .Include(r => r.Meals)
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }
}