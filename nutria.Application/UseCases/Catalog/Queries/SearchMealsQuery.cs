using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

public record SearchMealsQuery(string Name)
    : IRequest<List<Meal>>;

public class SearchMealsQueryHandler
    : IRequestHandler<SearchMealsQuery, List<Meal>>
{
    private readonly IMealRepository _mealRepository;

    public SearchMealsQueryHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<List<Meal>> Handle(
        SearchMealsQuery request,
        CancellationToken cancellationToken)
    {
        return await _mealRepository.SearchMealsByNameAsync(request.Name);
    }
}