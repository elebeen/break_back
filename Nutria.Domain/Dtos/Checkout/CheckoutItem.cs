namespace Nutria.Domain.Dtos.Checkout;

public class CheckoutItem
{
    public Guid MealId { get; set; }
    public int Quantity { get; set; }
}