using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetOrdersByUserAsync(Guid userId);

    Task<Order?> GetOrderDetailsAsync(Guid orderId);

    Task<List<Order>> GetOrdersByRestaurantAsync(Guid restaurantId);

    Task<OrderResponse?> GetOrderResponseAsync(Guid orderId);
    
}   