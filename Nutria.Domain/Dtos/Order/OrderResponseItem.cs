namespace Nutria.Domain.Dtos.Order;

public class OrderResponseItem
{
    public Guid Id { get; set; }
    public Guid MealId { get; set; }
    public string MealName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}