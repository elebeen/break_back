using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

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
        var user = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Id == request.UserId);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        
        return await _unitOfWork.Health.GetUserProfileWithHealthData(request.UserId);
    }
}