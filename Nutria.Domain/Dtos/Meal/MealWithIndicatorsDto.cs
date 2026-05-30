namespace Nutria.Domain.Dtos.Meal;

public class MealWithIndicatorsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    
    // Nueva información del restaurante
    public Guid RestaurantId { get; set; }
    public string RestaurantName { get; set; } = null!;
    
    // Indicadores de salud
    public bool ExceedsCalorieLimit { get; set; }
    public bool ExceedsSugarLimit { get; set; }
    public bool ExceedsSodiumLimit { get; set; }
    public bool HasAllergenWarning { get; set; }
    public List<string> SpecificWarnings { get; set; } = new();
}