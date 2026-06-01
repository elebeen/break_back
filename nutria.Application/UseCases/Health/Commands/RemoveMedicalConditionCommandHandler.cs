using MediatR;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Health.Commands;

public record RemoveMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<Unit>;

internal sealed record RemoveMedicalConditionCommandHandler : IRequestHandler<RemoveMedicalConditionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Health.RemoveConditionFromUser(request.UserId, request.ConditionId);
        await _unitOfWork.SaveChanges();

        return Unit.Value;
    }
}