namespace Nutria.Domain.Dtos.HealthProfile;

public class HealthProfileGetDto
{
    public string Goal { get; set; } = null!;

    public int? DailyCalorieTarget { get; set; }

    public int? DailySodiumLimitMg { get; set; }

    public int? DailySugarLimitG { get; set; }
}