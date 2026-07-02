namespace Nutria.Domain.Dtos.Meal;

public class EditMealDto
{
    public Guid RestaurantId { get; set; }     // Opcional, por si no se quiere cambiar

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Información nutricional
    public int Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatsG { get; set; }
    public decimal SodiumMg { get; set; }
    public decimal SugarG { get; set; }
    public decimal FiberG { get; set; }

    // Lista de ingredientes (se pueden actualizar)
    public List<int>? IngredientIds { get; set; }
}