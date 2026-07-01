using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Meals.Commands;

public record UpdateMealNutritionalInfoCommand(
    Guid MealId,
    int NewCalories,
    int NewSodiumMg,
    int NewSugarG
) : IRequest<bool>;

public class UpdateMealNutritionalInfoCommandHandler
    : IRequestHandler<UpdateMealNutritionalInfoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMealNutritionalInfoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateMealNutritionalInfoCommand request,
        CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Meal>();

        var meal = await repository.FindFirstAsync(x => x.Id == request.MealId);

        if (meal is null)
            return false;

        UpdateNutritionalInfo(meal, request);

        await repository.UpdateAsync(meal);

        await _unitOfWork.SaveChanges();

        return true;
    }

    private static void UpdateNutritionalInfo(
        Meal meal,
        UpdateMealNutritionalInfoCommand request)
    {
        if (meal.NutritionalInfo is null)
        {
            meal.NutritionalInfo = new NutritionalInfo
            {
                Id = Guid.NewGuid()
            };
        }

        meal.NutritionalInfo.Calories = request.NewCalories;
        meal.NutritionalInfo.SodiumMg = request.NewSodiumMg;
        meal.NutritionalInfo.SugarG = request.NewSugarG;
    }
}