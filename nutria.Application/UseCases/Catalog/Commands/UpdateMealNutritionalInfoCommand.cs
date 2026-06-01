using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Commands;

public record UpdateMealNutritionalInfoCommand(
    Guid MealId,
    int NewCalories,
    int NewSodiumMg,
    int NewSugarG
) : IRequest<bool>;

public class UpdateMealNutritionalInfoCommandHandler : IRequestHandler<UpdateMealNutritionalInfoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMealNutritionalInfoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateMealNutritionalInfoCommand request, CancellationToken cancellationToken)
    {
        // Buscamos el plato usando el método GetByIdAsync de tu IRepository
        var meal = await _unitOfWork.Repository<Meal>().FindFirstAsync(u => u.Id == request.MealId);
        
        if (meal == null) return false;

        // Si el plato no tiene inicializada la info nutricional, la creamos; si ya existe, la actualizamos
        if (meal.NutritionalInfo == null)
        {
            meal.NutritionalInfo = new NutritionalInfo
            {
                Id = Guid.NewGuid(),
                Calories = request.NewCalories,
                SodiumMg = request.NewSodiumMg,
                SugarG = request.NewSugarG
            };
        }
        else
        {
            meal.NutritionalInfo.Calories = request.NewCalories;
            meal.NutritionalInfo.SodiumMg = request.NewSodiumMg;
            meal.NutritionalInfo.SugarG = request.NewSugarG;
        }

        _unitOfWork.Repository<Meal>().Update(meal);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}