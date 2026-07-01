using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetAllMealsQuery()
    : IRequest<List<MealDto>>;

public class GetAllMealsQueryHandler
    : IRequestHandler<GetAllMealsQuery, List<MealDto>>
{
    private readonly IMealRepository _mealRepository;

    public GetAllMealsQueryHandler(
        IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<List<MealDto>> Handle(
        GetAllMealsQuery request,
        CancellationToken cancellationToken)
    {
        return await _mealRepository.GetAllMealsAsync();
    }
}