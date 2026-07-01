using MediatR;
using Nutria.Domain.Interfaces.Repositories;
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
        var repository = _unitOfWork.Repository<Meal>();

        var meal = await repository.FindFirstAsync(x => x.Id == request.MealId);

        if (meal is null)
            return false;

        meal.IsActive = false;

        await repository.UpdateAsync(meal);

        await _unitOfWork.SaveChanges();

        return true;
    }
}