using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<OrderResponse>> GetOrdersByUserAsync(Guid userId);

    Task<List<Order>> GetOrdersByRestaurantAsync(Guid restaurantId);

    Task<OrderResponse?> GetOrderDetailsByIdAsync(Guid orderId);
    
}   