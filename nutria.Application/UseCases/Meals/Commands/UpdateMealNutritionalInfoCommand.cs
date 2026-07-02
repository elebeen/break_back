using MediatR;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Meals.Commands;

public record UpdateMealCommand(Guid MealId, EditMealDto MealData) : IRequest<string>;

internal sealed class UpdateMealCommandHandler : IRequestHandler<UpdateMealCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateMealCommand request, CancellationToken cancellationToken)
    {
        var data = request.MealData;
        var mealId = request.MealId;

        // 1. Verificar que el plato existe
        var existingMeal = await _unitOfWork.Repository<Meal>().FindFirstAsync(m => m.Id == mealId);

        if (existingMeal == null)
        {
            throw new ArgumentException("The specified meal does not exist.");
        }

        // 2. Validar que el restaurante existe
        var restaurant = await _unitOfWork.Repository<Restaurant>()
            .FindFirstAsync(r => r.Id == data.RestaurantId);

        if (restaurant == null)
        {
            throw new ArgumentException("The specified restaurant does not exist.");
        }

        // 3. Validar ingredientes (si se enviaron)
        List<IngredientDto> existingIngredients = new();
        if (data.IngredientIds != null && data.IngredientIds.Count != 0)
        {
            existingIngredients = await _unitOfWork.Ingredients.GetIngredientsByIdsAsync(data.IngredientIds);

            if (existingIngredients.Count != data.IngredientIds.Count)
            {
                throw new ArgumentException("One or more specified ingredient IDs do not exist in the database.");
            }
        }

        // 4. Actualizar el Meal
        existingMeal.RestaurantId = data.RestaurantId;
        existingMeal.Name = data.Name;
        existingMeal.Description = data.Description;
        existingMeal.Price = data.Price;
        existingMeal.ImageUrl = data.ImageUrl;
        existingMeal.IsActive = data.IsActive;

        // 5. Actualizar NutritionalInfo
        if (existingMeal.NutritionalInfo != null)
        {
            existingMeal.NutritionalInfo.Calories = data.Calories;
            existingMeal.NutritionalInfo.ProteinG = data.ProteinG;
            existingMeal.NutritionalInfo.CarbsG = data.CarbsG;
            existingMeal.NutritionalInfo.FatsG = data.FatsG;
            existingMeal.NutritionalInfo.SodiumMg = data.SodiumMg;
            existingMeal.NutritionalInfo.SugarG = data.SugarG;
            existingMeal.NutritionalInfo.FiberG = data.FiberG;
        }
        else
        {
            // Por si acaso no tiene NutritionalInfo (caso raro)
            existingMeal.NutritionalInfo = new NutritionalInfo
            {
                Id = Guid.NewGuid(),
                MealId = mealId,
                Calories = data.Calories,
                ProteinG = data.ProteinG,
                CarbsG = data.CarbsG,
                FatsG = data.FatsG,
                SodiumMg = data.SodiumMg,
                SugarG = data.SugarG,
                FiberG = data.FiberG
            };
        }

        // 6. Actualizar Ingredientes (Many-to-Many)
        if (data.IngredientIds != null)
        {
            // Limpiar ingredientes actuales
            existingMeal.Ingredients.Clear();

            // Agregar los nuevos
            if (data.IngredientIds.Count > 0)
            {
                var newIngredients = existingIngredients.Select(dto => new Ingredient 
                { 
                    Id = dto.Id 
                }).ToList();

                existingMeal.Ingredients = newIngredients;
            }
        }

        // 7. Guardar cambios
        await _unitOfWork.SaveChanges();

        return "Meal updated successfully.";
    }
}