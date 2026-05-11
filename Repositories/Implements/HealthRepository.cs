using break_back.Models.Dtos.User;
using break_back.Models.Dtos.MedicalCondition;
using break_back.Models.Dtos.HealthProfile;
using break_back.Models;
using Microsoft.EntityFrameworkCore;

namespace break_back.Repositories.Implements;

public class HealthRepository : IHealthRepository
{
    private readonly Context _context;
    public HealthRepository(Context context)
    {
        _context = context;
    }

    public async  Task<UserHealthProfileDto?> GetUserHealthData(Guid userId)
    {   
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserHealthProfileDto
            {
                UserId = u.Id,
                FullName = u.FullName,        // o u.Name + u.LastName

                HealthProfile = u.HealthProfile != null ? new HealthProfileGetDto
                {
                    Goal = u.HealthProfile.Goal,
                    DailyCalorieTarget = u.HealthProfile.DailyCalorieTarget,
                    DailySugarLimitG = u.HealthProfile.DailySugarLimitG,
                    DailySodiumLimitMg = u.HealthProfile.DailySodiumLimitMg
                } : null,

                Conditions = u.Conditions
                    .Select(c => new MedicalConditionGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

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
                // Agrega más campos según tu entidad
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

        if (user?.Conditions.Any() == true)
        {
            var condition = user.Conditions.First();
            user.Conditions.Remove(condition);
        }
    }
}