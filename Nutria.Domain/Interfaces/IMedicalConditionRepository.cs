using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IMedicalConditionRepository
{
    Task<List<MedicalCondition>> GetAllAsync();

    Task<MedicalCondition?> GetByIdAsync(int id);

    Task<MedicalCondition?> GetByNameAsync(string name);

    Task<List<MedicalCondition>> GetByTypeAsync(string type);

    Task<bool> ExistsAsync(string name);
}