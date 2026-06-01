using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class OrderRepository: Repository<Order>, IOrderRepository
{
    public OrderRepository(AppdbContext context) : base(context) { }

    public async Task<List<Order>> GetOrdersByUserAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderDetailsAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Meal)
            .Include(o => o.Restaurant)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetOrdersByRestaurantAsync(Guid restaurantId)
    {
        return await _context.Orders
            .Where(o => o.RestaurantId == restaurantId)
            .ToListAsync();
    }
}