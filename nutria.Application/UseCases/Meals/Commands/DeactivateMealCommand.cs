using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Meals.Commands;

public record DeactivateMealCommand(Guid MealId) : IRequest<string>;

public class DeactivateMealCommandHandler : IRequestHandler<DeactivateMealCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(DeactivateMealCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Meal>();

        var meal = await repository.FindFirstAsync(x => x.Id == request.MealId);

        if (meal is null)
            return "Meal not found";

        meal.IsActive = false;

        await repository.UpdateAsync(meal);

        await _unitOfWork.SaveChanges();

        return "Meal deleted";
    }
}