using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IHealthRepository
{
    public Task<UserHealthProfileDto?> GetUserHealthData(Guid userId);
    public Task<List<MedicalConditionGetDto>> GetConditionsByUserId(Guid userId);
    public Task<MedicalCondition?> GetConditionByUserId(Guid userId, int conditionId);
    public Task RemoveConditionFromUser(Guid userId, int conditionId);
}






