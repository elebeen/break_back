using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Commands;

public record RemoveMedicalConditionCommand(Guid UserId, int ConditionId) : IRequest<Unit>;

public class RemoveMedicalConditionCommandHandler : IRequestHandler<RemoveMedicalConditionCommand, Unit>
{
    private readonly Context _context;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMedicalConditionCommandHandler(Context context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
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