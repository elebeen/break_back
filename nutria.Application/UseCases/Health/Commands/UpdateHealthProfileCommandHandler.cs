using MediatR;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record UpdateHealthProfileCommand(
    Guid UserId,
    HealthProfileCreateDto ProfileData
) : IRequest;

internal sealed class UpdateHealthProfileCommandHandler
    : IRequestHandler<UpdateHealthProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHealthProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Repository<HealthProfile>()
            .FindFirstAsync(h => h.UserId == request.UserId);

        if (profile is null)
            throw new Exception("Health profile not found.");

        profile.Goal = request.ProfileData.Goal;
        profile.DailyCalorieTarget = request.ProfileData.DailyCalorieTarget;
        profile.DailySodiumLimitMg = request.ProfileData.DailySodiumLimitMg;
        profile.DailySugarLimitG = request.ProfileData.DailySugarLimitG;

        await _unitOfWork.SaveChanges();
    }
}