namespace Nutria.Domain.Dtos.Checkout;

public class CheckoutRequest
{
    public Guid UserId { get; set; }
    public Guid RestaurantId { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public List<CheckoutItem> Items { get; set; } = new();
}