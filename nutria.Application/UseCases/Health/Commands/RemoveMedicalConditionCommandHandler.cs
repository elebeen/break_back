using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Health.Commands;

public record RemoveMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<Unit>;

internal sealed record RemoveMedicalConditionCommandHandler : IRequestHandler<RemoveMedicalConditionCommand, Unit>
{
    private readonly AppdbContext _appdbContext;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMedicalConditionCommandHandler(AppdbContext appdbContext, IUnitOfWork unitOfWork)
    {
        _appdbContext = appdbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = await _appdbContext.Users
            .Include(u => u.Conditions.Where(c => c.Id == request.ConditionId))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user?.Conditions.Any() == true)
        {
            var condition = user.Conditions.First();
            user.Conditions.Remove(condition);
            await _unitOfWork.SaveChanges();
        }

        return Unit.Value;
    }
}