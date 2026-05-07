using break_back.Models.Dtos.HealthProfile;
using break_back.Models.Dtos.MedicalCondition;

namespace break_back.Models.Dtos.User;

public class UserHealthProfileDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;   // o Email, etc.

    // HealthProfile
    public HealthProfileGetDto? HealthProfile { get; set; }

    // Condiciones médicas
    public List<MedicalConditionGetDto> Conditions { get; set; } = new();
}