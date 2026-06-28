using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record AddMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<string>;

internal sealed class AddMedicalConditionCommandHandler
    : IRequestHandler<AddMedicalConditionCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(AddMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Id == request.UserId);

        if (user is null)
            throw new ArgumentException("User not found.");

        var condition = await _unitOfWork.Repository<MedicalCondition>()
            .FindFirstAsync(c => c.Id == request.ConditionId);

        if (condition is null)
            throw new ArgumentException("Medical condition not found.");

        var alreadyAssigned = await _unitOfWork.Health
            .GetConditionByUserId(request.UserId, request.ConditionId);

        if (alreadyAssigned is not null)
            throw new ArgumentException("Medical condition already assigned.");

        user.Conditions.Add(condition);

        await _unitOfWork.SaveChanges();
        
        return "Medical condition added successfully.";
    }
}