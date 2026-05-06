namespace break_back.Models.Dtos.Ingredients;

public class IngredientDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsAllergen { get; set; }
}