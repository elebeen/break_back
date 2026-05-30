using MediatR;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record UpdateHealthProfileCommand(Guid UserId, HealthProfileCreateDto ProfileData) : IRequest<HealthProfile>;

public class UpdateHealthProfileCommandHandler : IRequestHandler<UpdateHealthProfileCommand, HealthProfile>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateHealthProfileCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<HealthProfile> Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
    {
        var newProfile = new HealthProfile
        {
            UserId = request.UserId,
            Goal = request.ProfileData.Goal,
            DailyCalorieTarget = request.ProfileData.DailyCalorieTarget,
            DailySodiumLimitMg = request.ProfileData.DailySodiumLimitMg,
            DailySugarLimitG = request.ProfileData.DailySugarLimitG,
        };

        _unitOfWork.Repository<HealthProfile>().Update(newProfile);
        await _unitOfWork.SaveChanges();

        return newProfile;
    }
}