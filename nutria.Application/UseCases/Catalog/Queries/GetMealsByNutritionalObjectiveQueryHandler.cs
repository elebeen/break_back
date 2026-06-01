using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;
public record GetMealsByNutritionalObjectiveQuery(string Objective)
    : IRequest<List<Meal>>;

public class GetMealsByNutritionalObjectiveQueryHandler
    : IRequestHandler<GetMealsByNutritionalObjectiveQuery, List<Meal>>
{
    private readonly IMealRepository _mealRepository;

    public GetMealsByNutritionalObjectiveQueryHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<List<Meal>> Handle(
        GetMealsByNutritionalObjectiveQuery request,
        CancellationToken cancellationToken)
    {
        var maxCalories = ResolveCalories(request.Objective);

        return await _mealRepository.GetMealsByCaloriesAsync(maxCalories);
    }

    private static int ResolveCalories(string objective)
    {
        return objective.ToLower() switch
        {
            "bajar de peso" => 500,
            "mantener peso" => 800,
            "ganar masa muscular" => 1200,
            _ => 2000
        };
    }
}