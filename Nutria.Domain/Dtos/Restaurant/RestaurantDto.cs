namespace Nutria.Domain.Dtos.Restaurant;

public class RestaurantDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactPhone { get; set; }

    public bool? IsActive { get; set; }
}