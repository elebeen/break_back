using break_back.Models;

namespace break_back.Services;

public interface IHealthService
{
    Task<HealthProfile> UpsertProfile(Guid userId, HealthProfile profileData);
    Task AddConditionToUser(Guid userId, int conditionId);
    Task RemoveConditionFromUser(Guid userId, int conditionId);
}