namespace Nutria.Domain.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsAllergen { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}


