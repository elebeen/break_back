namespace Nutria.Domain.Dtos.Meal;

public class MealCreateDto
{
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    // Información Nutricional obligatoria/opcional
    public int Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatsG { get; set; }
    public decimal? SodiumMg { get; set; }
    public decimal? SugarG { get; set; }
    public decimal? FiberG { get; set; }
    
    // LISTA DE INGREDIENTES EXISTENTES (NUEVA PROPIEDAD)
    public List<int> IngredientIds { get; set; } = new();
}