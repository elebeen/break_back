namespace Nutria.Domain.Dtos.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public Guid RestaurantId { get; set; }
    public string RestaurantName { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; } = null!;
    public string DeliveryAddress { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public List<OrderResponseItem> OrderItems { get; set; } = new();
}