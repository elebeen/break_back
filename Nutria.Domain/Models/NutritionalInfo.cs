namespace Nutria.Domain.Models;

public partial class NutritionalInfo
{
    public Guid Id { get; set; }

    public Guid MealId { get; set; }

    public int Calories { get; set; }

    public decimal ProteinG { get; set; }

    public decimal CarbsG { get; set; }

    public decimal FatsG { get; set; }

    public decimal? SodiumMg { get; set; }

    public decimal? SugarG { get; set; }

    public decimal? FiberG { get; set; }

    public virtual Meal Meal { get; set; } = null!;
}
