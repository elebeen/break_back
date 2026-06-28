using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
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
    
    public async Task<OrderResponse?> GetOrderResponseAsync(Guid orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .Select(o => new OrderResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = o.User.FullName,
                RestaurantId = o.RestaurantId,
                RestaurantName = o.Restaurant.Name,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                DeliveryAddress = o.DeliveryAddress,
                CreatedAt = o.CreatedAt,

                OrderItems = o.OrderItems.Select(oi => new OrderResponseItem
                {
                    Id = oi.Id,
                    MealId = oi.MealId,
                    MealName = oi.Meal.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    
}