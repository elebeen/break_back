namespace Nutria.Domain.Models;

public partial class HealthProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Goal { get; set; } = null!;

    public int? DailyCalorieTarget { get; set; }

    public int? DailySodiumLimitMg { get; set; }

    public int? DailySugarLimitG { get; set; }

    public virtual User User { get; set; } = null!;
}


