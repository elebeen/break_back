using MediatR;
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

        // 1. Verificar que el plato existe (Utilizamos el repositorio especializado para incluir las relaciones)
        // Nota: Asegúrate de usar o implementar una función que traiga el plato con sus relaciones en _unitOfWork.Meals
        var existingMeal = await _unitOfWork.Meals.GetMealWithNutritionalInfoAsync(mealId);

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

        // 3. Validar ingredientes (Deben ser de tipo 'Ingredient' con seguimiento de EF Core)
        List<Ingredient> trackingIngredients = new();
        if (data.IngredientIds != null && data.IngredientIds.Count != 0)
        {
            // El repositorio debe retornar List<Ingredient> sin .AsNoTracking()
            trackingIngredients = await _unitOfWork.Ingredients.GetIngredientsByIdsAsync(data.IngredientIds);

            if (trackingIngredients.Count != data.IngredientIds.Count)
            {
                throw new ArgumentException("One or more specified ingredient IDs do not exist in the database.");
            }
        }

        // 4. Actualizar las propiedades del Meal
        existingMeal.RestaurantId = data.RestaurantId;
        existingMeal.Name = data.Name;
        existingMeal.Description = data.Description;
        existingMeal.Price = data.Price;
        existingMeal.ImageUrl = data.ImageUrl;
        
        // Si data.IsActive es nullable, puedes usar ?? existingMeal.IsActive
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
            existingMeal.NutritionalInfo = new NutritionalInfo
            {
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

        // 6. Actualizar Ingredientes (Many-to-Many corregido)
        if (data.IngredientIds != null)
        {
            // Limpiamos la colección existente. Al estar bajo seguimiento, EF Core sabe
            // que debe eliminar estas relaciones específicas de la tabla intermedia "meal_ingredients"
            existingMeal.Ingredients.Clear();

            // Asignamos directamente la lista de entidades reales recuperadas de la BD
            foreach (var ingredient in trackingIngredients)
            {
                existingMeal.Ingredients.Add(ingredient);
            }
        }

        // 7. Guardar cambios
        // EF Core generará automáticamente los UPDATES para Meal y NutritionalInfo, 
        // y se encargará de sincronizar (INSERTs/DELETEs) la tabla "meal_ingredients"
        await _unitOfWork.SaveChanges();

        return "Meal updated successfully.";
    }
}