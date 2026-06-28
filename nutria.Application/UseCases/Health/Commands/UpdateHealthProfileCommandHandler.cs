using MediatR;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record UpdateHealthProfileCommand(
    HealthProfileCreateDto ProfileData
) : IRequest<string>;

internal sealed class UpdateHealthProfileCommandHandler
    : IRequestHandler<UpdateHealthProfileCommand,  string>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHealthProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Id == request.ProfileData.UserId);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        
        var profile = await _unitOfWork.Repository<HealthProfile>()
            .FindFirstAsync(h => h.UserId == request.ProfileData.UserId);
        
        var isNewProfile = false;

        if (profile is null)
        {
            isNewProfile = true;
            profile = new HealthProfile
            {
                Id = Guid.NewGuid(),
                UserId = request.ProfileData.UserId
            };
        }
        
        profile.Goal = request.ProfileData.Goal;
        profile.DailyCalorieTarget = request.ProfileData.DailyCalorieTarget;
        profile.DailySodiumLimitMg = request.ProfileData.DailySodiumLimitMg;
        profile.DailySugarLimitG = request.ProfileData.DailySugarLimitG;

        if (isNewProfile)
        {
            await _unitOfWork.Repository<HealthProfile>().AddAsync(profile);
        }
        else
        {
            await _unitOfWork.Repository<HealthProfile>().UpdateAsync(profile);
        }
        
        await _unitOfWork.SaveChanges();

        return "Profile updated successfully.";
    }
}