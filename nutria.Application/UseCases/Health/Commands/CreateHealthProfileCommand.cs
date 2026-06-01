using MediatR;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record CreateHealthProfileCommand(
    Guid UserId,
    HealthProfileCreateDto ProfileData
) : IRequest;

internal sealed class CreateHealthProfileCommandHandler
    : IRequestHandler<CreateHealthProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateHealthProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateHealthProfileCommand request, CancellationToken cancellationToken)
    {
        var existingProfile = await _unitOfWork.Repository<HealthProfile>()
            .FindFirstAsync(h => h.UserId == request.UserId);

        if (existingProfile is not null)
            throw new Exception("Health profile already exists.");

        var profile = new HealthProfile
        {
            UserId = request.UserId,
            Goal = request.ProfileData.Goal,
            DailyCalorieTarget = request.ProfileData.DailyCalorieTarget,
            DailySodiumLimitMg = request.ProfileData.DailySodiumLimitMg,
            DailySugarLimitG = request.ProfileData.DailySugarLimitG
        };

        await _unitOfWork.Repository<HealthProfile>()
            .AddAsync(profile);

        await _unitOfWork.SaveChanges();
    }
}