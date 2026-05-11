using break_back.Models.Dtos.MedicalCondition;
using break_back.Models.Dtos.User;
using break_back.Models;

namespace break_back.Repositories;

public interface IHealthRepository
{
    public Task<UserHealthProfileDto?> GetUserHealthData(Guid userId);
    public Task<List<MedicalConditionGetDto>> GetConditionsByUserId(Guid userId);
    public Task<MedicalCondition?> GetConditionByUserId(Guid userId, int conditionId);
    public Task RemoveConditionFromUser(Guid userId, int conditionId);
}