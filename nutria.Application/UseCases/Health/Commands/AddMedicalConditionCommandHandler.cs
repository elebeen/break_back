using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record AddMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<Unit>;

public class AddMedicalConditionCommandHandler : IRequestHandler<AddMedicalConditionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddMedicalConditionCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(AddMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = _unitOfWork.Repository<User>().FindbyGuid(request.UserId);
        var condition = _unitOfWork.Repository<MedicalCondition>().FindById(request.ConditionId);

        if (user != null && condition != null)
        {
            user.Conditions.Add(condition);
            await _unitOfWork.SaveChanges();
        }

        return Unit.Value;
    }
}