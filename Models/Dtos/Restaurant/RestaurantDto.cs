namespace break_back.Models.Dtos.Restaurant;

public class RestaurantDto
{
    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactPhone { get; set; }

    public bool? IsActive { get; set; }
}