using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IMedicalConditionRepository
{
    public Task<List<MedicalConditionGetDto>> GetConditionsByUserId(Guid userId);
    public Task<MedicalCondition?> GetConditionByUserId(Guid userId, int conditionId);
    public Task RemoveConditionFromUser(Guid userId, int conditionId);
}