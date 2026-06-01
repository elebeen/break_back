using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

//  Definimos la petición (Query) que recibirá el objetivo del usuario
public record GetMealsByNutritionalObjectiveQuery(string Objective) : IRequest<List<Meal>>;

// El Handler que ejecutará la lógica de negocio usando el repositorio real de platos
public class GetMealsByNutritionalObjectiveQueryHandler : IRequestHandler<GetMealsByNutritionalObjectiveQuery, List<Meal>>
{
    private readonly IMealRepository _mealRepository;

    public GetMealsByNutritionalObjectiveQueryHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<List<Meal>> Handle(GetMealsByNutritionalObjectiveQuery request, CancellationToken cancellationToken)
    {
        // Lógica de negocio: Asignamos un rango de calorías inteligente según la meta elegida por el cliente
        int maxCalories = request.Objective.ToLower() switch
        {
            "bajar de peso" => 500,    
            "mantener peso" => 800,    
            "ganar masa muscular" => 1200, 
            _ => 2000                
        };

        var meals = await _mealRepository.GetMealsByCaloriesAsync(maxCalories);
        
        return meals ?? new List<Meal>();
    }
}