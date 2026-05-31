using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class MedicalConditionRepository : Repository<MedicalCondition>, IMedicalConditionRepository
{
    public MedicalConditionRepository(AppdbContext context)
        : base(context)
    {
    }

    public async Task<List<MedicalCondition>> GetAllAsync()
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MedicalCondition?> GetByIdAsync(int id)
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<MedicalCondition?> GetByNameAsync(string name)
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<List<MedicalCondition>> GetByTypeAsync(string type)
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .Where(c => c.Type == type)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.MedicalConditions
            .AnyAsync(c => c.Name == name);
    }
}