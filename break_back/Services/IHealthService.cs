using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Models;

namespace break_back.Services;

public interface IHealthService
{
    Task<UserHealthProfileDto> GetProfile(Guid userId);
    Task<HealthProfile> UpdateProfile(Guid userId, HealthProfileCreateDto profileData);
    Task AddConditionToUser(Guid userId, int conditionId);
    Task RemoveConditionFromUser(Guid userId, int conditionId);
    Task RemoveAllConditionsFromUser(Guid userId);
}