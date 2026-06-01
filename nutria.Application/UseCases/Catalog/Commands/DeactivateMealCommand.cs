using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Commands;

public record DeactivateMealCommand(Guid MealId) : IRequest<bool>;

public class DeactivateMealCommandHandler : IRequestHandler<DeactivateMealCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateMealCommand request, CancellationToken cancellationToken)
    {
        var meal = await _unitOfWork.Repository<Meal>()
            .FindFirstAsync(x => x.Id == request.MealId);
        
        if (meal == null) return false;

        await _unitOfWork.Repository<Meal>().Update(meal);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}