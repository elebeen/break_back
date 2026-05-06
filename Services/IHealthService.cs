using break_back.Models;
using break_back.Models.Dtos.HealthProfileDtos;

namespace break_back.Services;

public interface IHealthService
{
    Task<HealthProfile> UpsertProfile(Guid userId, HealthProfileCreateDto profileData);
    Task AddConditionToUser(Guid userId, int conditionId);
    Task RemoveConditionFromUser(Guid userId, int conditionId);
}