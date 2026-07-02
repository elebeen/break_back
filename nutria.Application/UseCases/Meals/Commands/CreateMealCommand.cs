using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Meals.Commands;

public record CreateMealCommand(MealCreateDto MealData) : IRequest<string>;

internal sealed class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        var data = request.MealData;

        // 1. Validar si el restaurante existe
        var restaurant = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(r => r.Id == data.RestaurantId);
        if (restaurant == null)
        {
            throw new ArgumentException("The specified restaurant does not exist.");
        }

        // 2. Buscar los ingredientes existentes en la base de datos (Deben ser de tipo 'Ingredient')
        List<Ingredient> existingIngredients = new();
        if (data.IngredientIds != null && data.IngredientIds.Count != 0)
        {
            // El repositorio debe retornar List<Ingredient> rastreados por EF Core
            existingIngredients = await _unitOfWork.Ingredients.GetIngredientsByIdsAsync(data.IngredientIds);

            // Validación: Verificar si todos los IDs enviados realmente existen
            if (existingIngredients.Count != data.IngredientIds.Count)
            {
                throw new ArgumentException("One or more specified ingredient IDs do not exist in the database.");
            }
        }

        var mealId = Guid.NewGuid();

        // 3. Construir la entidad mapeando los ingredientes recuperados
        var newMeal = new Meal
        {
            Id = mealId,
            RestaurantId = data.RestaurantId,
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            ImageUrl = data.ImageUrl,
            IsActive = true,
            
            NutritionalInfo = new NutritionalInfo
            {
                MealId = mealId,
                Calories = data.Calories,
                ProteinG = data.ProteinG,
                CarbsG = data.CarbsG,
                FatsG = data.FatsG,
                SodiumMg = data.SodiumMg,
                SugarG = data.SugarG,
                FiberG = data.FiberG
            },

            // CORRECCIÓN CLAVE: Asignamos directamente la lista de entidades rastreadas.
            // Al no hacer un "new Ingredient", EF Core sabe que ya existen en la BD.
            Ingredients = existingIngredients
        };

        // 4. Guardar y persistir cambios
        await _unitOfWork.Repository<Meal>().AddAsync(newMeal);
        await _unitOfWork.SaveChanges();

        return "Meal registered and associated with its ingredients successfully.";
    }
}