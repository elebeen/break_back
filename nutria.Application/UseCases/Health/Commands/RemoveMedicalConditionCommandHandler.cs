using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Health.Commands;

public record RemoveMedicalConditionCommand(
    Guid UserId,
    int ConditionId
) : IRequest;

internal sealed class RemoveMedicalConditionCommandHandler
    : IRequestHandler<RemoveMedicalConditionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var condition = await _unitOfWork.Health
            .GetConditionByUserId(request.UserId, request.ConditionId);

        if (condition is null)
            return;

        await _unitOfWork.Health
            .RemoveConditionFromUser(request.UserId, request.ConditionId);

        await _unitOfWork.SaveChanges();
    }
}