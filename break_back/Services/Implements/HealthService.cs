using break_back.Models;
using break_back.Models.Dtos.HealthProfile;
using break_back.Models.Dtos.User;
using break_back.Repositories;
using Microsoft.EntityFrameworkCore;

namespace break_back.Services.Implements;

public class HealthService : IHealthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Context _context;

    public HealthService(IUnitOfWork unitOfWork,  Context context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<HealthProfile> UpdateProfile(Guid userId, HealthProfileCreateDto profileData)
    {
        var newProfile = new HealthProfile()
        {
            UserId = userId,
            Goal = profileData.Goal,
            DailyCalorieTarget = profileData.DailyCalorieTarget,
            DailySodiumLimitMg = profileData.DailySodiumLimitMg,
            DailySugarLimitG = profileData.DailySugarLimitG,
        };
    
        _unitOfWork.Repository<HealthProfile>().Update(newProfile);
        await _unitOfWork.SaveChanges();
        
        return newProfile;
    }

    public async Task AddConditionToUser(Guid userId, int conditionId)
    {
        var user = _unitOfWork.Repository<User>().FindbyGuid(userId);

        var condition = _unitOfWork.Repository<MedicalCondition>().FindById(conditionId); //

        if (user != null && condition != null)
        {
            user.Conditions.Add(condition);
            await _unitOfWork.SaveChanges(); // EF Core inserta en user_medical_conditions
        }
    }

    public async Task RemoveConditionFromUser(Guid userId, int conditionId)
    {
       // var user = _unitOfWork.Repository<User>().FindbyGuid(userId);

        // obtener conditions de un usuario
        //var condition = await _unitOfWork.HealthRepository.GetConditionByUserId(userId, conditionId);

        //if (user != null && condition != null)
        //{
        //    await _unitOfWork.HealthRepository.RemoveConditionFromUser(userId, conditionId);
        //}
        
        var user = await _context.Users
            .Include(u => u.Conditions
                .Where(c => c.Id == conditionId))   // Solo cargamos la que queremos
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Conditions.Any() == true)
        {
            var condition = user.Conditions.First();
            user.Conditions.Remove(condition);
        
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task<UserHealthProfileDto> GetProfile(Guid userId)
    {
        return await _unitOfWork.HealthRepository.GetUserHealthData(userId);
    }

    public async Task RemoveAllConditionsFromUser(Guid userId)
    {
        throw new Exception("This method is not implemented");
    }
}