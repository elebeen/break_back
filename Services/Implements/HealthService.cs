using break_back.Models;
using Microsoft.EntityFrameworkCore;
using break_back.Repositories;

namespace break_back.Services.Implements;

public class HealthService : IHealthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Context _context; // Usamos el context para facilitar Includes

    public HealthService(IUnitOfWork unitOfWork, Context context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<HealthProfile> UpsertProfile(Guid userId, HealthProfile profileData)
    {
        // 1. Buscar si ya existe el perfil para el usuario
        var profile = await _context.HealthProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            // Crear nuevo perfil
            profileData.UserId = userId;
            _unitOfWork.Repository<HealthProfile>().Add(profileData);
            profile = profileData;
        }
        else
        {
            // Actualizar campos existentes
            profile.Goal = profileData.Goal;
            profile.DailyCalorieTarget = profileData.DailyCalorieTarget;
            profile.DailySodiumLimitMg = profileData.DailySodiumLimitMg;
            profile.DailySugarLimitG = profileData.DailySugarLimitG;
            _unitOfWork.Repository<HealthProfile>().Update(profile);
        }

        await _unitOfWork.SaveChanges(); //
        return profile;
    }

    public async Task AddConditionToUser(Guid userId, int conditionId)
    {
        var user = await _context.Users
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == userId); //

        var condition = _unitOfWork.Repository<MedicalCondition>().FindById(conditionId); //

        if (user != null && condition != null && !user.Conditions.Contains(condition))
        {
            user.Conditions.Add(condition);
            await _unitOfWork.SaveChanges(); // EF Core inserta en user_medical_conditions
        }
    }

    public async Task RemoveConditionFromUser(Guid userId, int conditionId)
    {
        var user = await _context.Users
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var condition = user?.Conditions.FirstOrDefault(c => c.Id == conditionId);

        if (user != null && condition != null)
        {
            user.Conditions.Remove(condition);
            await _unitOfWork.SaveChanges();
        }
    }
}