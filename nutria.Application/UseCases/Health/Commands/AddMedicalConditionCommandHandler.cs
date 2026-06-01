using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record AddMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<Unit>;

internal sealed record AddMedicalConditionCommandHandler
    : IRequestHandler<AddMedicalConditionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddMedicalConditionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(AddMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork
            .Repository<User>()
            .GetByIdAsync(request.UserId);

        var condition = await _unitOfWork
            .Repository<MedicalCondition>()
            .Query()
            .FirstOrDefaultAsync(x => x.Id == request.ConditionId, cancellationToken);

        if (user != null && condition != null)
        {
            user.Conditions.Add(condition);
            await _unitOfWork.SaveChanges();
        }

        return Unit.Value;
    }
}