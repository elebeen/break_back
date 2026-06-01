using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record AddMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest;

internal sealed class AddMedicalConditionCommandHandler
    : IRequestHandler<AddMedicalConditionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .FindFirstAsync(u => u.Id == request.UserId);

        if (user is null)
            throw new Exception("User not found.");

        var condition = await _unitOfWork.Repository<MedicalCondition>()
            .FindFirstAsync(c => c.Id == request.ConditionId);

        if (condition is null)
            throw new Exception("Medical condition not found.");

        var alreadyAssigned = await _unitOfWork.Health
            .GetConditionByUserId(request.UserId, request.ConditionId);

        if (alreadyAssigned is not null)
            return;

        user.Conditions.Add(condition);

        await _unitOfWork.SaveChanges();
    }
}