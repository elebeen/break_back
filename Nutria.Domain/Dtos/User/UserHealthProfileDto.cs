using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Dtos.MedicalCondition;

namespace Nutria.Domain.Dtos.User;

public class UserHealthProfileDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;   // o Email, etc.

    // HealthProfile
    public HealthProfileGetDto? HealthProfile { get; set; }

    // Condiciones médicas
    public List<MedicalConditionGetDto> Conditions { get; set; } = [];
}