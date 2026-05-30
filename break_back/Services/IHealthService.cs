/*using break_back.Models;
using break_back.Models.Dtos.HealthProfile;
using break_back.Models.Dtos.User;

namespace break_back.Services;

public interface IHealthService
{
    Task<UserHealthProfileDto> GetProfile(Guid userId);
    Task<HealthProfile> UpdateProfile(Guid userId, HealthProfileCreateDto profileData);
    Task AddConditionToUser(Guid userId, int conditionId);
    Task RemoveConditionFromUser(Guid userId, int conditionId);
    Task RemoveAllConditionsFromUser(Guid userId);
}*/