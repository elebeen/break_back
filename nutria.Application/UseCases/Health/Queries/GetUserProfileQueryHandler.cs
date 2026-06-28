using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Health.Queries;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserHealthProfileDto?>;

internal sealed record GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, UserHealthProfileDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserProfileQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<UserHealthProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Health.GetUserHealthData(request.UserId);
    }
}