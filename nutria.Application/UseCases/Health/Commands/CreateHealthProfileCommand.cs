using MediatR;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record CreateHealthProfileCommand(Guid UserId, HealthProfileCreateDto ProfileData) : IRequest<bool>;

internal sealed record CreateHealthProfileCommandHandler : IRequestHandler<UpdateHealthProfileCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateHealthProfileCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
    {
        var newProfile = new HealthProfile
        {
            UserId = request.UserId,
            Goal = request.ProfileData.Goal,
            DailyCalorieTarget = request.ProfileData.DailyCalorieTarget,
            DailySodiumLimitMg = request.ProfileData.DailySodiumLimitMg,
            DailySugarLimitG = request.ProfileData.DailySugarLimitG,
        };

        await _unitOfWork.Repository<HealthProfile>().AddAsync(newProfile);
        await _unitOfWork.SaveChanges();

        return true;
    }
}