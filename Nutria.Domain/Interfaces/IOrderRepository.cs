using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersByUserAsync(Guid userId);

    Task<Order?> GetOrderDetailsAsync(Guid orderId);

    Task<List<Order>> GetOrdersByRestaurantAsync(Guid restaurantId);
}