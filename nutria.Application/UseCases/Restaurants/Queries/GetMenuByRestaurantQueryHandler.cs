using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Restaurants.Queries;

public record GetMenuByRestaurantQuery(Guid RestaurantId) : IRequest<IEnumerable<MealDto>>;

internal sealed record GetMenuByRestaurantQueryHandler : IRequestHandler<GetMenuByRestaurantQuery, IEnumerable<MealDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMenuByRestaurantQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<MealDto>> Handle(GetMenuByRestaurantQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Meals.GetMealsByRestaurantAsync(request.RestaurantId);
    }
}