namespace Nutria.Domain.Dtos.Restaurant;

public class CreateRestaurantDto
{
    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactPhone { get; set; }

    public bool? IsActive { get; set; }
}