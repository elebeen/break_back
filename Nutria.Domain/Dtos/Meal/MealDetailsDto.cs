using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Dtos.NutritionalInfo;

namespace Nutria.Domain.Dtos.Meal;

public class MealDetailsDto
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }

    public NutritionalInfoDto? NutritionalInfo { get; set; }
    public List<IngredientDto> Ingredients { get; set; } = new();
}