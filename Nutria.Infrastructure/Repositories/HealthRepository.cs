using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class HealthRepository : IHealthRepository
{
    private readonly AppdbContext _appdbContext;
    public HealthRepository(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async  Task<UserHealthProfileDto?> GetUserHealthData(Guid userId)
    {   
        return await _appdbContext.Users
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
        return await _appdbContext.Users
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
        return await _appdbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Conditions)           // Navega a través de la tabla intermedia
            .FirstOrDefaultAsync(c => c.Id == conditionId);
    }
    
    public async Task RemoveConditionFromUser(Guid userId, int conditionId)
    {
        var user = await _appdbContext.Users
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