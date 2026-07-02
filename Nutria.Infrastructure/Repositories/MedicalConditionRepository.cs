using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class MedicalConditionRepository : Repository<MedicalCondition>, IMedicalConditionRepository
{
    public MedicalConditionRepository(AppdbContext context) : base(context) { }
    
    public async Task<List<MedicalConditionGetDto>> GetConditionsByUserId(Guid userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Conditions)
            .Select(c => new MedicalConditionGetDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
            })
            .ToListAsync();
    }

    public async Task<MedicalCondition?> GetConditionByUserId(Guid userId, int conditionId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Conditions)           // Navega a través de la tabla intermedia
            .FirstOrDefaultAsync(c => c.Id == conditionId);
    }
    
    public async Task RemoveConditionFromUser(Guid userId, int conditionId)
    {
        var user = await _context.Users
            .Include(u => u.Conditions
                .Where(c => c.Id == conditionId))   // Solo cargamos la que queremos
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null && user.Conditions.Count != 0)
        {
            var condition = user.Conditions.First();
            user.Conditions.Remove(condition);
        }
    }

    public async Task<List<MedicalConditionGetDto>> GetAllConditions()
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .Select(c => new MedicalConditionGetDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
            })
            .ToListAsync();
    }
}