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
}