using MediatR;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Health.Commands;

public record RemoveMedicalConditionCommand(
    Guid UserId,
    int ConditionId
) : IRequest<string>;

internal sealed class RemoveMedicalConditionCommandHandler
    : IRequestHandler<RemoveMedicalConditionCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(RemoveMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var condition = await _unitOfWork.Health
            .GetConditionByUserId(request.UserId, request.ConditionId);

        if (condition is null)
            throw new ArgumentException($"Condition does not exist.");

        await _unitOfWork.Health
            .RemoveConditionFromUser(request.UserId, request.ConditionId);

        await _unitOfWork.SaveChanges();
        
        return "Medical condition deleted successfully.";
    }
}