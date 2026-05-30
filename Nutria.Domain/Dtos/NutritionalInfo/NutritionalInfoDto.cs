namespace Nutria.Domain.Dtos.NutritionalInfo;

public class NutritionalInfoDto
{
    public int Calories { get; set; }

    public decimal ProteinG { get; set; }

    public decimal CarbsG { get; set; }

    public decimal FatsG { get; set; }

    public decimal? SodiumMg { get; set; }

    public decimal? SugarG { get; set; }

    public decimal? FiberG { get; set; }
}